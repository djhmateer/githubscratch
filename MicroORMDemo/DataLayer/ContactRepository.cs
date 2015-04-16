using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;

namespace MicroOrmDemo.DataLayer {
    public class ContactRepository : IContactRepository {
        private IDbConnection db =
            new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString);

        public Contact Find(int id) {
            // Use Anonymous types typically
            // SingleOrDefault so if its not found it will return a null
            return db.Query<Contact>("SELECT * FROM Contacts WHERE Id = @Id", new { Id = id }).SingleOrDefault();
        }

        public List<Contact> GetAll() {
            return db.Query<Contact>("SELECT * FROM Contacts").ToList();
        }

        public Contact Add(Contact contact) {
            var sql = "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES (@FirstName, @LastName, @Email, @Company, @Title); " +
                       "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = db.Query<int>(sql, contact).Single();
            contact.Id = id;
            return contact;
        }

        public Contact Update(Contact contact) {
            var sql =
             "UPDATE Contacts " +
             "SET FirstName = @FirstName, " +
             "    LastName  = @LastName, " +
             "    Email     = @Email, " +
             "    Company   = @Company, " +
             "    Title     = @Title " +
             "WHERE Id = @Id";
            db.Execute(sql, contact);
            return contact;
        }

        public void Remove(int id) {
            db.Execute("DELETE FROM Contacts WHERE ID = @Id", new { id });
        }

        // Multiple select statements
        public Contact GetFullContact(int id) {
            var sql = "SELECT * FROM Contacts WHERE Id = @Id; " +
                      "SELECT * From Addresses WHERE ContactID = @Id";

            using (var multipleResults = db.QueryMultiple(sql, new { id })) {
                var contact = multipleResults.Read<Contact>().SingleOrDefault();
                var addresses = multipleResults.Read<Address>().ToList();

                if (contact != null && addresses != null) {
                    contact.Addresses.AddRange(addresses);
                }

                return contact;
            }
        }

        public void Save(Contact contact) {
            using (var txScope = new TransactionScope()) {
                if (contact.IsNew) {
                    Add(contact);
                }
                else {
                    Update(contact);
                }

                foreach (var addr in contact.Addresses.Where(a => !a.IsDeleted)) {
                    addr.ContactId = contact.Id;

                    if (addr.IsNew) {
                        Add(addr);
                    }
                    else {
                        Update(addr);
                    }
                }

                // if delete an address off an existing contact
                foreach (var addr in contact.Addresses.Where(a => a.IsDeleted)) {
                    db.Execute("DELETE FROM Addresses WHERE Id = @Id", new { addr.Id });
                }

                txScope.Complete();
            }
        }

        public Address Add(Address address) {
            var sql = @"
                        INSERT INTO [dbo].[Addresses]
                               ([ContactId]
                               ,[AddressType]
                               ,[StreetAddress]
                               ,[City]
                               ,[StateId]
                               ,[PostalCode])
                         VALUES
                               (@ContactID
                               ,@AddressType
                               ,@StreetAddress
                               ,@City
                               ,@StateId
                               ,@PostalCode);" +
            "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = db.Query<int>(sql, address).Single();
            address.Id = id;
            return address;
        }

        public Address Update(Address address) {
            var sql = @"
                        UPDATE [dbo].[Addresses]
                           SET [ContactId] = @ContactId
                              ,[AddressType] = @AddressType
                              ,[StreetAddress] = @StreetAddress
                              ,[City] = @City
                              ,[StateId] = @StateId
                              ,[PostalCode] = @PostalCode
                         WHERE Id = " + address.Id;
            db.Execute(sql, address);
            return address;
        }
    }

    public class Contact {
        // So when a new contact is created, a blank list of addresses is created
        public Contact() {
            this.Addresses = new List<Address>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }

        public List<Address> Addresses { get; private set; }

        // Convenience property
        public bool IsNew {
            get {
                return Id == default(int);
            }
        }
    }

    public class Address {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string AddressType { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PostalCode { get; set; }

        // Convenience properties
        internal bool IsNew {
            get {
                return Id == default(int);
            }
        }

        public bool IsDeleted { get; set; }
    }

    public interface IContactRepository {
        Contact Find(int id);
        List<Contact> GetAll();
        Contact Add(Contact contact);
        Contact Update(Contact contact);
        void Remove(int id);

        Contact GetFullContact(int id);
        void Save(Contact contact);
    }
}
