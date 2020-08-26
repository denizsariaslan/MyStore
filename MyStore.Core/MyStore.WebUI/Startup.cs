using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyStore.WebUI.Startup))]
namespace MyStore.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
