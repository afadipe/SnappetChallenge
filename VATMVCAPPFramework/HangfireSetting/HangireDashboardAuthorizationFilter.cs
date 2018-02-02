using Hangfire.Dashboard;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VATMVCAPPFramework.HangfireSetting
{
    public class HangireDashboardAuthorizationFilter: IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var owinContext = new OwinContext(context.GetOwinEnvironment());
            // Allow all authenticated users to see the Dashboard (potentially dangerous).
           if(owinContext.Authentication.User.Identity.IsAuthenticated)
                return owinContext.Authentication.User.IsInRole("PortalAdmin");
            return false;

        }
    }
}