using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MicroOrmDemo.DataLayer {
    public class ContactRepository : IContactRepository {
        private IDbConnection db =
            new SqlConnection(ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString);

        public Contact Find(int id) {
            // Use Anonymous types typically
            // SingleOrDefault so if its not found it will return a null
            return db.Query<Contact>("SELECT * FROM Contacts WHERE Id = @Id", new {Id = id}).SingleOrDefault();
        }

        public List<Contact> GetAll() {
            return db.Query<Contact>("SELECT * FROM Contacts").ToList();
        }

        public Contact Add(Contact contact) {
            var sql = "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES                     (@FirstName, @LastName, @Email, @Company, @Title); " +
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

        public void Remove(int id)
        {
            db.Execute("DELETE FROM Contacts WHERE ID = @Id", new {id});
        }

        public Contact GetFullContact(int id) {
            throw new NotImplementedException();
        }

        public void Save(Contact contact) {
            throw new NotImplementedException();
        }
    }

    public class Contact {
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

        public bool IsNew {
            get {
                return this.Id == default(int);
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

        internal bool IsNew {
            get {
                return this.Id == default(int);
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
