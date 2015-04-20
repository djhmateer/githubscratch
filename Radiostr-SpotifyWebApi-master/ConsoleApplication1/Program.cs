using Radiostr.SpotifyWebApi;
using Radiostr.SpotifyWebApi.Cache;
using Radiostr.SpotifyWebApi.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    class Program {
        static void Main() {
            Console.WriteLine("hello");
            Thing().Wait();
            //Thing();
            Console.ReadLine();
        }

        static async Task<string> Thing(){
            var http = new RestHttpClient(new System.Net.Http.HttpClient());
            var memCache = new RuntimeMemoryCache(System.Runtime.Caching.MemoryCache.Default);
            var clientCredentialsAuthApi = new ClientCredentialsAuthorizationApi(http,System.Configuration.ConfigurationManager.AppSettings, memCache);
            var api = new PlaylistsApi(http,clientCredentialsAuthApi);

            var playlists =  await api.GetPlaylists("davemateer");
            Trace.TraceInformation(playlists.ToString());
            return "xx";
        }
    }
}
