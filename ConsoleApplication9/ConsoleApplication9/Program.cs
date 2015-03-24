using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleApplication9 {
    class Program {
        static void Main(string[] args) {
            // explore async/await for when I do many API calls

            Task t = MainAsync();
            t.Wait();
            Console.WriteLine("done");
        }

        static async Task MainAsync()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            const int n = 50;
            //ServicePointManager.DefaultConnectionLimit = 5;
            Task<string>[] tasks = new Task<string>[n];
            for (int i = 0; i < n; i++)
            {
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

            // call takes a few seconds
            //var url = "http://www.programgood.net/SearchView.aspx?q=task";
            
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            //request.ServicePoint.ConnectionLimit = 5;
            string result;
            using (var wresponse = await request.GetResponseAsync()){
                result = wresponse.ResponseUri.ToString();
            }

            //WebResponse ws = await request.GetResponseAsync();
            //return ws.ResponseUri.ToString()
            Console.WriteLine("CallAPI exit " + i);
            return result;
        }

        //static async Task<int> LongRunningOperation(int i) 
        //{
        //    Console.WriteLine("start inside lro " + i);
        //    var url =
        //        "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";
        //    // 6.5secs synch

        //    var stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    var result = CallAPI(url);
        //    Console.WriteLine("end lro " + i);

        //    stopWatch.Stop();
        //    TimeSpan ts = stopWatch.Elapsed;
        //    string elapsedTime = String.Format("{0:0}", ts.TotalMilliseconds);
        //    return 1;
        //}

        //private static string CallAPI(string url) {
        //    int errorCount = 0;
        //    string text = null;
        //    bool done = false;
        //    while (!done) {
        //        try {
        //            var request = (HttpWebRequest)WebRequest.Create(url);
        //            request.Accept = "application/json";

        //            var response = (HttpWebResponse)request.GetResponse();
        //            using (var sr = new StreamReader(response.GetResponseStream())) {
        //                text = sr.ReadToEnd();
        //            }

        //            done = true;
        //        } catch (WebException ex) {
        //            Console.WriteLine("error");
        //            Thread.Sleep(200);
        //            errorCount++;
        //            if (errorCount == 200)
        //                throw;
        //        }
        //    }
        //    return text;
        //}
    }
}
