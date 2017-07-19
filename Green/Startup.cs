using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Green.Startup))]
namespace Green
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
