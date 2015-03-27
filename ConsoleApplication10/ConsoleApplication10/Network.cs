using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace ConsoleApplication10 {
    class NetworkStuff {
        //http://blogs.msdn.com/b/henrikn/archive/2012/02/16/httpclient-is-here.aspx
        //http://blogs.msdn.com/b/webdev/archive/2012/08/26/asp-net-web-api-and-httpclient-samples.aspx
        //static string _address = "http://api.worldbank.org/countries?format=json";
        //static string _address = "http://www.programgood.netXXXX/CategoryView,category,c.aspx";

        

        // davemateeroutlook
        //9CO7ZPJ3MTA2LGSUN 
        private static string _address = "http://developer.echonest.com/api/v4/artist/biographies?api_key=9CO7ZPJ3MTA2LGSUN&id=spotify:artist:4Z8W4fKeB5YxbusRsdQVPb";

        // this is their dev API key
        //private static string _address = "http://developer.echonest.com/api/v4/artist/biographies?api_key=FILDTEOIK2HBORODV&id=spotify:artist:4Z8W4fKeB5YxbusRsdQVPb";
        static void Main() {
            ServicePointManager.DefaultConnectionLimit = 5;
            const int n = 20;
            var tasks = new Task<string>[n];
            for (int i = 0; i < n; i++) {
                tasks[i] = CallAPI(i);
            }

            Task.WaitAll(tasks);
            Console.WriteLine("done");
            Console.ReadLine();
        }

        static async Task<string> CallAPI(int i){
            var keepTrying = true;
            while (keepTrying){
                try{
                    var client = new HttpClient();

                    HttpResponseMessage response = await client.GetAsync(_address);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(i + result.Substring(0, 100));
                    keepTrying = false;
                    return result;
                }
                catch (HttpRequestException hre){
                    Console.WriteLine(i + hre.Message);
                    System.Threading.Thread.Sleep(500);
                }
                catch (Exception ex){
                    Console.WriteLine("here" + i + ex.Message);
                }
            }
            return null;
        }
    }
}
