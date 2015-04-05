using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using StackExchange.Profiling;

namespace MiniProfilerTest {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MiniProfiler.Settings.Results_Authorize = IsUserAllowedToSeeMiniProfilerUI;
            MiniProfiler.Settings.Results_List_Authorize = IsUserAllowedToSeeMiniProfilerUI;
        }

        private bool IsUserAllowedToSeeMiniProfilerUI(HttpRequest httpRequest) {
            // Implement your own logic for who 
            // should be able to access ~/mini-profiler-resources/results
            //var principal = httpRequest.RequestContext.HttpContext.User;
            //return principal.IsInRole("Developer");
            return true;
        }

        //protected void Application_Start(object sender, EventArgs e) {}

        protected void Application_BeginRequest() {
            if (Request.IsLocal) {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest() {
            MiniProfiler.Stop();
        }
    }
}
