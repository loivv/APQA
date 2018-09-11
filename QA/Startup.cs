using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QA.Startup))]
namespace QA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
