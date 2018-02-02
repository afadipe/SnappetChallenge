using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VATMVCAPPFramework.Data.EntityBase;

namespace VATMVCAPPFramework.Data.IdentityModel
{
    public class RolePermission : BaseEntityWithAudit
    {
        public Int64 PermissionId { get; set; }
        public Int64 RoleId { get; set; }
    }
}
