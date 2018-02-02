using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.IdentityModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace VATMVCAPPFramework.Data.IdentityService
{
    

    public class ApplicationUserManager: UserManager<ApplicationUser,Int64>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser,Int64> store) : base(store)
        {
        }
    }
}
