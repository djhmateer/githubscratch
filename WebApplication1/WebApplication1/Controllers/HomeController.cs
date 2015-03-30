using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication1.Controllers {

    public class Thing {
        public Response response { get; set; }

        public class Response {
            public Status status { get; set; }
            public int start { get; set; }
            public int total { get; set; }
            public Biography[] biographies { get; set; }
        }

        public class Status {
            public string version { get; set; }
            public int code { get; set; }
            public string message { get; set; }
        }

        public class Biography {
            public string text { get; set; }
            public string site { get; set; }
            public string url { get; set; }
            public License license { get; set; }
            public bool truncated { get; set; }
        }

        public class License {
            public string type { get; set; }
            public string attribution { get; set; }
            public string attributionurl { get; set; }
            public string url { get; set; }
            public string version { get; set; }
        }
    }


    public class HomeController : Controller {
        private static string _address = "http://developer.echonest.com/api/v4/artist/biographies?api_key=9CO7ZPJ3MTA2LGSUN&id=spotify:artist:4Z8W4fKeB5YxbusRsdQVPb";

        public async Task<ActionResult> Index() {
            var time = DateTime.Now;
            ServicePointManager.DefaultConnectionLimit = 5;
            const int n = 15;
            var tasks = new Task<string>[n];
            for (int i = 0; i < n; i++) {
                tasks[i] = CallAPI(i);
            }

            // at this point all tasks will be running at the same time

            // await all tasks
            await Task.WhenAll();
            Debug.WriteLine(Actual(time), "After all tasks");

            // do stuff
            // check that all results have been returned!

            for (int i = 0; i < n; i++)
            {
                var things = JsonConvert.DeserializeObject<Thing>(tasks[0].Result);
                var url = things.response.biographies[6].url; 
                Debug.WriteLine(url);
            }
           
            //ViewBag.stuff = tasks[0].Result;
            return View();
        }

        async Task<string> CallAPI(int i) {
            var time = DateTime.Now;
            string result;
            using (var client = new HttpClient()) {
                // ConfigureAwat - do not capture the current ASP.NET request context
                var response = await client.GetAsync(_address).ConfigureAwait(false);

                // Not on the original context here, instead we're running on the thread pool
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