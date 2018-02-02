using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.Data.IdentityService
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, Int64, ApplicationUserRole>
    {
        public ApplicationRoleStore(APPContext context) : base(context)
        {
        }
    }
}
