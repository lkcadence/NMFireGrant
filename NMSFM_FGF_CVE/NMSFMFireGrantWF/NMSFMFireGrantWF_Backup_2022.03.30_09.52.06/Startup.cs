using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NMSFMFireGrantWF.Startup))]
namespace NMSFMFireGrantWF
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
