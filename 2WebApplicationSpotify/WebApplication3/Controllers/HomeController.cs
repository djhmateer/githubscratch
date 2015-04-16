using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace WebApplication3.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            var uri = "https://api.spotify.com/v1/search?q=queen&type=artist&limit=10";
            var client = new HttpClient();

            // Make a call
            var httpResponse = client.GetAsync(uri);
            // Get the result
            var json = httpResponse.Result.Content.ReadAsStringAsync().Result;

            var searchResult = JsonConvert.DeserializeObject<SearchResult>(json);

            return View(searchResult);
        }
    }

    public class SearchResult {
        public class ExternalUrls {
            public string spotify { get; set; }
        }

        public class Followers {
            public object href { get; set; }
            public int total { get; set; }
        }

        public class Image {
            public string height { get; set; }
            public string url { get; set; }
            public string width { get; set; }
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