using System;
using VATMVCAPPFramework.Data.IdentityModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace VATMVCAPPFramework.Data.IdentityService
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, Int64>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
           : base(userManager, authenticationManager)
        {
        }
    }
}
