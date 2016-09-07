using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCWebsite.Startup))]
namespace MVCWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
