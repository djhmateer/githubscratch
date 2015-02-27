using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Mvc;

namespace WebOAuth.Controllers {
    public class ProfilesController : Controller {
        public ActionResult Me() {
            var returnURL = "/Profiles/Me";
            var authHelper = new AuthHelper();
            var urlToCallSpotify = authHelper.DoAuth(returnURL, this);
            // if no AccessToken in current Session redirect to Spotify
            if (urlToCallSpotify != null)
                return Redirect(urlToCallSpotify);

            // we are authorised now (after logging into Spotify) so can display the user's personal details
            var url = "https://api.spotify.com/v1/me";
            var spotifyHelper = new SpotifyHelper();
            var access_token = Session["AccessToken"].ToString();
            var result2 = spotifyHelper.CallSpotifyAPIPassingToken(access_token, url);

            var meReponse = JsonConvert.DeserializeObject<MeResponse>(result2);
            meReponse.access_token = access_token;
            return View(meReponse);
        }

        public ActionResult SpotifyCallback(string code) {
            bool keepTrying = true;
            string resultContent = "";
            while (keepTrying) {
                // Have now code authorization code (which can be exchanged for an access token)
                var client_id = "285954b953174282a79a1cd7597ac5e6";
                var client_secret = "c839c4a6c9dc4192b099dcdf5cc6bc90";

                var url = "https://accounts.spotify.com/api/token";

                // Request access and refresh tokens
                var postData = new Dictionary<string, string>{
                    {"grant_type", "authorization_code"},
                    {"code", code},
                    {"redirect_uri", GetRedirectUriWithServerName()},
                    {"client_id", client_id},
                    {"client_secret", client_secret}
                };

                HttpContent content = new FormUrlEncodedContent(postData.ToArray());

                var client = new HttpClient();
                var httpResponse = client.PostAsync(url, content);
                var result = httpResponse.Result;
                resultContent = result.Content.ReadAsStringAsync().Result;

                // Catching gateway timeouts or strange stuff from Spotify
                if (result.IsSuccessStatusCode) {
                    keepTrying = false;
                }
            }

            var obj = JsonConvert.DeserializeObject<AccessToken>(resultContent, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            });
            var access_token = obj.access_token;

            // Set access token in session
            Session["AccessToken"] = access_token;
            // Get return URL from session state
            var returnURL = Session["ReturnURL"].ToString();

            return Redirect(returnURL);
        }

        private string GetRedirectUriWithServerName() {
            return "http://" + Request.Url.Authority + "/Profiles/SpotifyCallback";
        }
        
    }

    public class SpotifyHelper {
        public string CallSpotifyAPIPassingToken(string access_token, string url) {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            var httpResponse = client.GetAsync(url);
            var result = httpResponse.Result.Content.ReadAsStringAsync().Result;
            return result;
        }

        public static string CallAPI(string url = "") {
            int errorCount = 0;
            string text = null;
            bool done = false;
            while (!done) {
                try {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Accept = "application/json";

                    var response = (HttpWebResponse)request.GetResponse();

                    using (var sr = new StreamReader(response.GetResponseStream())) {
                        text = sr.ReadToEnd();
                    }

                    done = true;
                } catch (WebException ex) {
                    Debug.WriteLine("Exception: " + ex.Message);
                    Thread.Sleep(100);
                    errorCount++;
                    if (errorCount == 10)
                        throw;
                }
            }

            if (String.IsNullOrEmpty(text)) throw new InvalidOperationException();
            return text;
        }
    }

    public class MeResponse {
        public class ExternalUrls {
            public string spotify { get; set; }
        }

        public class Followers {
            public object href { get; set; }
            public int total { get; set; }
        }

        public string country { get; set; }
        public object display_name { get; set; }
        public string email { get; set; }
        public ExternalUrls external_urls { get; set; }
        public Followers followers { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<object> images { get; set; }
        public string product { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        // dave added
        public string access_token { get; set; }
    }

    public class AuthHelper {
        public string DoAuth(string returnURL, Controller controller) {
            if (controller.Session["AccessToken"] == null) {
                var client_id = "285954b953174282a79a1cd7597ac5e6";
                var response_type = "code";
                var scope = "user-read-private user-read-email playlist-modify-public ";

                var xx = "http://" + controller.Request.Url.Authority + "/Profiles/SpotifyCallback";
                var url = String.Format("https://accounts.spotify.com/authorize/?client_id={0}&response_type={1}&scope={3}&redirect_uri={2}",
                                        client_id, response_type, xx, scope);

                controller.Session["ReturnURL"] = returnURL;
                return url;
            }
            return null;
        }
    }

    internal class AccessToken {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }

        public AuthenticationToken ToPOCO() {
            AuthenticationToken token = new AuthenticationToken();
            token.AccessToken = this.access_token;
            token.ExpiresOn = DateTime.Now.AddSeconds(this.expires_in);
            token.RefreshToken = this.refresh_token;
            token.TokenType = this.token_type;

            return token;
        }
    }

    public class AuthenticationToken {
        private string accessToken;

        /// <summary>
        /// An access token that can be provided in subsequent calls, for example to Spotify Web API services. 
        /// 
        /// refreshes the token automatically if it has expired
        /// </summary>
        public string AccessToken {
            get {
                if (HasExpired)
                    Refresh();

                return accessToken;
            }
            set {
                accessToken = value;
            }
        }

        /// <summary>
        /// How the access token may be used: always "Bearer". 
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// The date/time that this token will become invalid
        /// </summary>
        public DateTime ExpiresOn { get; set; }

        /// <summary>
        /// A token that can be sent to the Spotify Accounts service in place of an authorization code. 
        /// (When the access code expires, send a POST request to the Accounts service /api/token endpoint, but 
        /// use this code in place of an authorization code. A new access token and a new refresh token will be returned.) 
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Determines if this token has expired
        /// </summary>
        public bool HasExpired { get { return DateTime.Now > ExpiresOn; } }

        /// <summary>
        /// Updates this token if it has expired
        /// </summary>
        public async void Refresh() {
            //var token = await SpotifyWebAPI.Authentication.GetAccessToken(this.RefreshToken);
            //this.accessToken = token.accessToken;
            //this.ExpiresOn = token.ExpiresOn;
            //this.RefreshToken = token.RefreshToken;
            //this.TokenType = this.TokenType;
        }
    }

}