using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;

[assembly: OwinStartupAttribute(typeof(VATMVCAPPFramework.Startup))]
namespace VATMVCAPPFramework
{
    public partial class Startup
    {
        public static IDataProtectionProvider DataProtectionProvider { get; set; }
        public void Configuration(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();
            ConfigureAuth(app);
            HangfireConfig.ConfigureHangfire(app);
            HangfireConfig.InitializeJobs();

        }
    }
}
