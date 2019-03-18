using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Patronus.Startup))]
namespace Patronus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
