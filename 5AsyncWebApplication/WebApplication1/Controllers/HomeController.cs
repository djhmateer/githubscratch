using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApplication1.Controllers {
    public class HomeController : Controller {
        private static string _address = "http://developer.echonest.com/api/v4/artist/biographies?api_key=9CO7ZPJ3MTA2LGSUN&id=spotify:artist:4Z8W4fKeB5YxbusRsdQVPb";
        //private static string _address = "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";

        public async Task<ActionResult> Index() {
            var time = DateTime.Now;
            ServicePointManager.DefaultConnectionLimit = 5;
            const int n = 5;
            var tasks = new Task<string>[n];
            for (int i = 0; i < n; i++) {
                tasks[i] = CallAPI(i);
            }

            // at this point all tasks will be running at the same time
            await Task.WhenAll(tasks);
            Debug.WriteLine(Actual(time), "After all tasks");
           
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
            Debug.WriteLine(Actual(time), "Finished " + i);
            return result;
        }

        string Actual(DateTime time) {
            return (DateTime.Now - time).TotalMilliseconds.ToString();
        }
    }
}