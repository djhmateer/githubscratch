using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication9 {
    class Program2 {
        static void Main2() {
            // Create task and start it.
            // ... Wait for it to complete.
            Task task = new Task(ProcessDataAsync);
            task.Start();
            task.Wait();
            Console.ReadLine();
        }

        static async void ProcessDataAsync() {
            // Start the HandleFile method.
            Task<int> task = HandleFileAsync("f:\\temp\\enable1.txt");
            Task<string> task2 = DoSomething();

            Console.WriteLine("Please wait patiently while I do something important.");
            Thread.Sleep(1200);
            Console.WriteLine("after sleep");

            // Wait for the HandleFile task to complete.
            // ... Display its results.
            int x = await task;
            string y = await task2;
            Console.WriteLine("Count: " + x);
            Console.WriteLine("y: " + y);
        }

        static async Task<int> HandleFileAsync(string file) {
            Console.WriteLine("HandleFile enter");
            int count = 0;

            using (var reader = new StreamReader(file)) {
                string v = await reader.ReadToEndAsync();

                count += v.Length;
                Thread.Sleep(500);
                for (int i = 0; i < 10000; i++) {
                    int x = v.GetHashCode();
                    if (x == 0) count--;
                }
            }
            Console.WriteLine("HandleFile exit");
            return count;
        }

        static async Task<string> DoSomething() {
            Console.WriteLine("DoSomething enter");
            string url = "http://imgur.com/gallery/VcBfl.json";
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);

            WebResponse ws = await request.GetResponseAsync();
            Console.WriteLine("DoSomething exit");
            return ws.ResponseUri.ToString(); ;
        }
    }
}
