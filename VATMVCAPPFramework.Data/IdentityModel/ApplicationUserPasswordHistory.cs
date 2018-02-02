using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.IdentityModel
{
    public  class ApplicationUserPasswordHistory: BaseEntityWithAudit
    {
        public Int64 UserId { get; set; }
        public string HashPassword { get; set; }

        public string PasswordSalt { get; set; }
    }
}
