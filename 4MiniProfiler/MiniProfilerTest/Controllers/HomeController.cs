using Dapper;
using StackExchange.Profiling;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace MiniProfilerTest.Controllers {
    public class HomeController : Controller {
        MiniProfiler mp = MiniProfiler.Current;

        public ActionResult Index() {
            using (mp.Step("Set page title")) {
                ViewBag.Title = "Home Page";
            }

            using (mp.Step("Doing complex stuff")) {
                using (mp.Step("Step A")) { 
                    Thread.Sleep(100);
                }
                using (mp.Step("Step B")) { 
                    Thread.Sleep(250);
                }
            }

            using (var db = GetOpenConnection()) {
                var contacts = db.GetList<Contact>().ToList();
                return View(contacts);
            }
        }

        private IDbConnection GetOpenConnection() {
            DbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["contactsDB"].ConnectionString);
            cnn.Open();

            // wrap the connection with a profiling connection that tracks timings 
            return new StackExchange.Profiling.Data.ProfiledDbConnection(cnn, MiniProfiler.Current);
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
    }
}