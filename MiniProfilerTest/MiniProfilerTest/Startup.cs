using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MiniProfilerTest.Startup))]
namespace MiniProfilerTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
