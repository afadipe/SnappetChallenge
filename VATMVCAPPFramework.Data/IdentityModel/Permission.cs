using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.IdentityModel
{
    public class Permission : BaseEntityWithAudit
    {
        public string Name { get; set; }
        public string Code { get; set; }
        
    }
}
