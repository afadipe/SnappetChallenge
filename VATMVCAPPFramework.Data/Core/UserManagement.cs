using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VATMVCAPPFramework.Data.Core
{
    public class ClassTeacherUserModel
    {
        public long Id { get; set; }
        public long StaffID { get; set; }

        public long UserId { get; set; }

        public string LastName { get; set; }

        public string StaffFullName { get; set; }
    }
    public class UserRoleInfo
    {
        public Int64 RoleId { get; set; }
        public string Name { get; set; }
    }
    public class UserPremissionAndRole
    {
        public string PermissionName { get; set; }

        public string PermissionCode { get; set; }

        public string RoleName { get; set; }
    }
}
