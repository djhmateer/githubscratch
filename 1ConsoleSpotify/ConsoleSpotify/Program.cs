using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;


namespace ConsoleSpotify {
    class Program {
        static void Main() {
            var uri = "https://api.spotify.com/v1/search?q=queen&type=artist&limit=2";
            var client = new HttpClient();
            var httpResponse = client.GetAsync(uri);
            var json = httpResponse.Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(json);
            //var searchResult = JsonConvert.DeserializeObject<SearchResult3>(json);
            //foreach (var item in searchResult.artists.items) {
            //    Console.WriteLine(item.name);
            //    if (item.images.Count > 0)
            //        Console.WriteLine(item.images[0].url);
            //}

            Console.ReadLine();
        }
    }


    public class SearchResult3 {
        public Artists artists { get; set; }

        public class Artists {
            public string href { get; set; }
            public Item[] items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total { get; set; }
        }

        public class Item {
            public External_Urls external_urls { get; set; }
            public Followers followers { get; set; }
            public string[] genres { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public List<Image> images { get; set; }
            public string name { get; set; }
            public int popularity { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls {
            public string spotify { get; set; }
        }

        public class Followers {
            public object href { get; set; }
            public int total { get; set; }
        }

        public class Image {
            public int height { get; set; }
            public string url { get; set; }
            public int width { get; set; }
        }

    }

    public class SearchResult2 {
        public class ExternalUrls {
            public string spotify { get; set; }
        }

        public class Followers {
            public object href { get; set; }
            public int total { get; set; }
        }

        public class Item {
            public ExternalUrls external_urls { get; set; }
            public Followers followers { get; set; }
            public List<object> genres { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public List<Image> images { get; set; }
            public string name { get; set; }
            public int popularity { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class Image {
            public string height { get; set; }
            public string url { get; set; }
            public string width { get; set; }

        }

        public class Artists {
            public string href { get; set; }
            public List<Item> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total { get; set; }
        }

        public Artists artists { get; set; }
    }

    public class SearchResult {
        public class ExternalUrls {
            public string spotify { get; set; }
        }

        public class Followers {
            public object href { get; set; }
            public int total { get; set; }
        }

        public class Item {
            public ExternalUrls external_urls { get; set; }
            public Followers followers { get; set; }
            public List<object> genres { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public List<Image> images { get; set; }
            public string name { get; set; }
            public int popularity { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class Image {
            public string height { get; set; }
            public string url { get; set; }
            public string width { get; set; }

        }

        public class Artists {
            public string href { get; set; }
            public List<Item> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total { get; set; }
        }

        public Artists artists { get; set; }
    }
}
