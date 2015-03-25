using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleApplication9 {
    class Program {
        static void Main() {
            //Sync();
            //explore async/await for when I do many API calls
            Task t = MainAsync();
            t.Wait();
            Console.WriteLine("done");
        }

        //static void Sync() {
        //    var stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    var url = "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";
        //    for (int i = 0; i < 50; i++) {
        //        HttpWebRequest request = HttpWebRequest.CreateHttp(url);
        //        string result;
        //        using (var wresponse = request.GetResponse()) {
        //            result = wresponse.ResponseUri.ToString();
        //        }
        //    }
        //    stopWatch.Stop();
        //    TimeSpan ts = stopWatch.Elapsed;
        //    string elapsedTime = String.Format("{0:0}", ts.TotalMilliseconds);
        //    Console.WriteLine("Total: " + elapsedTime);
        //}

        static async Task MainAsync() {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            const int n = 50;
            ServicePointManager.DefaultConnectionLimit = 20;
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

        static async Task<string> CallAPI(int i) {
            Console.WriteLine("CallAPI enter " + i);
            var url = "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            string result;
            using (var wresponse = await request.GetResponseAsync()) {
                result = wresponse.ResponseUri.ToString();
            }
            Console.WriteLine("CallAPI exit " + i);
            return result;
        }
    }
}
