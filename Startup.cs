using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BettingSiteNet.Startup))]
namespace BettingSiteNet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
