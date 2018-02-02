using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VATMVCAPPFramework.Data.Constant
{
   public  class AppConstant
    {

        public const string PortalAdminRoleName = "PortalAdmin";
        public const string SchoolAdminRoleName = "SchoolAdmin";
        public const string CampusAdminRoleName = "CampusAdmin";
        public const string FetchUserPermissionAndRole = "FetchUserPermissionAndRole";
        public const string DeletePermissionByRoleID = "DeletePermissionByRoleID";
        public const string FetchUserRoleByUserId = "SpGetUserRole";
        public const string DeleteUserRoleByUserId = "SpDeleteUserRoleByUserId";
        public const string GetClassSubject = "dbo.GetClassSubject";
        public const string GetSchool = "spGetSchool";
        public const string spGetProgrammeSession = "spGetProgrammeSession";
        public const int  MaxFileUploadSize = 1024 * 1024;
    }
}
