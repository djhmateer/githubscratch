using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ORMStuff {
    class Program {
        static void Main() {
            string connectionString = ConfigurationManager.ConnectionStrings["ContactsDB"].ConnectionString;

            // ADO.NET
            var stuff1 = new List<Contact>();
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(null, connection)) {
                connection.Open();
                command.CommandText = String.Format("SELECT * FROM Contacts");
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {

                        var id = reader.GetInt32(reader.GetOrdinal("Id"));
                        var firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        var lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        var email = reader.GetString(reader.GetOrdinal("Email"));
                        var company = reader.GetString(reader.GetOrdinal("Company"));
                        var title = reader.GetString(reader.GetOrdinal("Title"));
                        var contact = new Contact { Id = id, FirstName = firstName, LastName = lastName, Email = email, Company = company, Title = title };
                        stuff1.Add(contact);
                    }
                }
            }


            // Dapper
            IDbConnection db = new SqlConnection(connectionString);
            var stuff2 = db.Query<Contact>("SELECT * FROM Contacts");
            var stuff5 = db.Query<Contact>("SELECT * FROM Contacts ORDER BY LastName");


            // Dapper with SimpleCRUD
            var stuff3 = db.GetList<Contact>();
            var stuff4 = db.GetList<Contact>("ORDER BY LastName");


        }
    }

    // For SimpleCRUD
    [Table("Contacts")]
    public class Contact {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
    }
}
