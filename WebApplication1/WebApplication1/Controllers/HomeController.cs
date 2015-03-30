using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApplication1.Controllers {
    public class HomeController : Controller {
        private static string _address = "http://developer.echonest.com/api/v4/artist/biographies?api_key=9CO7ZPJ3MTA2LGSUN&id=spotify:artist:4Z8W4fKeB5YxbusRsdQVPb";

        public async Task<ActionResult> Index() {
            var time = DateTime.Now;
            ServicePointManager.DefaultConnectionLimit = 5;
            const int n = 5;
            var tasks = new Task<string>[n];
            for (int i = 0; i < n; i++) {
                tasks[i] = CallAPI(i);
            }

            // as we don't want to wait synchronously for an async method
            await Task.WhenAll();

            Debug.WriteLine(Actual(time), "After all tasks");
            ViewBag.stuff = tasks[0].Result;
            return View();
        }

        async Task<string> CallAPI(int i) {
            var time = DateTime.Now;
            string result;
            using (var client = new HttpClient()) {
                var response = await client.GetAsync(_address).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            Debug.WriteLine(Actual(time), "After " + i);
            return result;
        }

        string Actual(DateTime time) {
            return (DateTime.Now - time).TotalMilliseconds.ToString();
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
}