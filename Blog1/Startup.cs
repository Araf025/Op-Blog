using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blog1.Startup))]
namespace Blog1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
