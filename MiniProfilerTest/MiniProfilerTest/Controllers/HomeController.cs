using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Dapper;
using StackExchange.Profiling;

namespace MiniProfilerTest.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            var profiler = MiniProfiler.Current; // it's ok if this is null

            using (profiler.Step("Set page title")) {
                ViewBag.Title = "Home Page";
            }

            using (profiler.Step("Doing complex stuff")) {
                using (profiler.Step("Step A")) { // something more interesting here
                    Thread.Sleep(100);
                }
                using (profiler.Step("Step B")) { // and here
                    Thread.Sleep(250);
                }
            }

            using (var db = GetOpenConnection()) {
                var contacts = db.GetList<Contact>().ToList();
                return View(contacts);
            }
        }

        private IDbConnection GetOpenConnection() {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["contactsDB"].ConnectionString);
            connection.Open();
            return connection;
        }

        public Contact Get(int id) {
            using (var db = GetOpenConnection()) {
                return db.Get<Contact>(id);
            }
        }

        public IEnumerable<Contact> GetList() {
            using (var db = GetOpenConnection()) {
                return db.GetList<Contact>();
            }
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    // used for SimpleCRUD
    [Table("Contacts")]
    public class Contact {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }

        //Additional properties not in database
        //[Editable(false)]
        //public bool IsNew {
        //    get {
        //        return this.Id == default(int);
        //    }
        //}
    }
}