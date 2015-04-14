using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebOAuth.Startup))]
namespace WebOAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
