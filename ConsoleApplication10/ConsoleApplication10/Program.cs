using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication10 {
    class Program {
        static void Main2() {
            Task t = RunAsync(1);
            Console.WriteLine("starting 1");
            Thread.Sleep(2000);

            Task t2 = RunAsync(2);
            Console.WriteLine("starting 2");

            t.Wait();
            Console.WriteLine("done 1");
            t2.Wait();
            Console.WriteLine("done 2");
        }

        static async Task RunAsync(int i) {
            for (int j = 0; j < 50; j++){
                Console.Write(i);
                // If I do this (which isn't async) then it blocks
                Thread.Sleep(100);
                //await Task.Run(() => Thread.Sleep(10));
                //await Task.Delay(100);
            }
           
        }
    }
}

 //using (var client = new HttpClient()) {
 //           var url = "https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO";

 //               //client.BaseAddress = new Uri("https://api.spotify.com/v1/albums/?ids=3KuXEGcqLcnEYWnn3OEGy0,0eFHYz8NmK75zSplL5qlfM,0lw68yx3MhKflWFqCsGkIs,0HcHPBu9aaF1MxOiZmUQTl,5eJTvSeghTKoqN3Ly4TqEf,1Dh27pjT3IEdiRG9Se5uQn,6AyUVv7MnxxTuijp4WmrhO");
 //               //client.DefaultRequestHeaders.Accept.Clear();
 //               //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

 //               HttpResponseMessage response = await client.GetAsync(url);
 //               if (response.IsSuccessStatusCode){
 //                   var result = await response.Content.ReadAsStringAsync();
 //                   //Console.WriteLine(result.Substring(0,400));
 //                   //Product product = await response.Content.ReadAsAsync>Product>();
 //                   //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
 //               }

 //           }
