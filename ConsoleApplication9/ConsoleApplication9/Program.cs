using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApplication9 {
    class Program {
        static void Main() {
            //explore async/await for when I do many API calls
            ServicePointManager.DefaultConnectionLimit = 50;
            Sync();
            Async().Wait();
            Console.WriteLine("done");
        }

        static void Sync() {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var url = "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";
            for (int i = 0; i < 10; i++) {
                var time = DateTime.Now;
                Console.WriteLine("Starting " + i);
                HttpWebRequest request = HttpWebRequest.CreateHttp(url);
                string text;
                using (var response = request.GetResponse()) {
                    using (var sr = new StreamReader(response.GetResponseStream())) {
                        text = sr.ReadToEnd();
                    }
                }
                Console.WriteLine("end in " + Actual(time));
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:0}", ts.TotalMilliseconds);
            Console.WriteLine("Total: " + elapsedTime);
        }

        static async Task Async() {
            var time = DateTime.Now;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
           
            const int n = 10;
            Task<string>[] tasks = new Task<string>[n];
            for (int i = 0; i < n; i++) {
                tasks[i] = CallAPI(i);
            }

            await tasks[0];
            Console.WriteLine("0 - " + Actual(time));
            await tasks[1];
            Console.WriteLine("1 - " + Actual(time));
            await tasks[2];
            Console.WriteLine("2 - " + Actual(time));
            await tasks[3];
            Console.WriteLine("3 - " + Actual(time));
            await tasks[4];
            Console.WriteLine("4 - " + Actual(time));

            Task.WaitAll(tasks);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:0}", ts.TotalMilliseconds);
            Console.WriteLine("Total: " + elapsedTime);
        }

        static string Actual(DateTime time) {
            return (DateTime.Now - time).TotalMilliseconds.ToString();
        }

        static async Task<string> CallAPI(int i) {
            Console.WriteLine("CallAPI enter " + i);
            var url = "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";

            var client = new HttpClient();
            //var response = await client.GetAsync(url).ConfigureAwait(false);
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            //var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var text = await response.Content.ReadAsStringAsync();
            
            return "";
        }
    }
}
