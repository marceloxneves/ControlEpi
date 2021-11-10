using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TitansMVC.Startup))]
namespace TitansMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
