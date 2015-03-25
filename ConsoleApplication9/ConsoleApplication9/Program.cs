using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleApplication9 {
    class Program {
        static void Main() {
            //explore async/await for when I do many API calls

            // what if there is an error?

            Sync();
            //Async();
            Console.WriteLine("done");
        }

        static void Sync() {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var url = "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";
            for (int i = 0; i < 5; i++) {
                Console.WriteLine("Starting " + i);
                HttpWebRequest request = HttpWebRequest.CreateHttp(url);
                string text;
                using (var response = request.GetResponse()) {
                    using (var sr = new StreamReader(response.GetResponseStream())) {
                        text = sr.ReadToEnd();
                    }
                }
                Console.WriteLine("result of " + i + " " + text.Substring(0,400));
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:0}", ts.TotalMilliseconds);
            Console.WriteLine("Total: " + elapsedTime);
        }

        static void Async() {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            ServicePointManager.DefaultConnectionLimit = 50;
            const int n = 5;
            Task<string>[] tasks = new Task<string>[n];
            for (int i = 0; i < n; i++) {
                tasks[i] = CallAPI(i);
            }

            Task.WaitAll(tasks);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:0}", ts.TotalMilliseconds);
            Console.WriteLine("Total: " + elapsedTime);
        }

        // Switch to HttpClient - .NET45.. http://stackoverflow.com/a/25612028/26086
        static async Task<string> CallAPI(int i) {
            Console.WriteLine("CallAPI enter " + i);
            var url = "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            string text;
            using (var response = await request.GetResponseAsync()) {
                using (var sr = new StreamReader(response.GetResponseStream())) {
                    text = sr.ReadToEnd();
                }
            }

            Console.WriteLine("CallAPI exit " + i + text.Substring(0,400));
            return text;
        }
    }
}
