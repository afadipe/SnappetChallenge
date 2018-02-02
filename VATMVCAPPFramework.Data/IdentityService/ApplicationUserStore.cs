using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using VATMVCAPPFramework.Data.IdentityModel;


namespace VATMVCAPPFramework.Data.IdentityService
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, Int64, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore(APPContext context) : base(context)
        {
        }
    }
}
