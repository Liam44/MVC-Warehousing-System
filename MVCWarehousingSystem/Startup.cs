using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCWarehousingSystem.Startup))]
namespace MVCWarehousingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
