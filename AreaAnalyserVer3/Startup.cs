using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AreaAnalyserVer3.Startup))]
namespace AreaAnalyserVer3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
