using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    public partial class userspmigration : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("dbo.SpGetUserRole",
                   p => new
                   {
                       UserId = p.Int(0)
                   },
                   body:
                   @"select  
	              a.AspNetRoleId RoleId,
                  e.[Name] 
                  FROM [dbo].[AspNetUserRole] a
                  inner join [dbo].[AspNetRole] e on  a.[AspNetRoleId]=e.AspNetRoleId
                  where (@UserId=0 or a.AspNetUserId=@UserId)
              ");

            CreateStoredProcedure("dbo.SpDeleteUserRoleByUserId",
           p => new
           {
               UserId = p.Int(0)
           },
           body:
           @"Delete from AspNetUserRole where AspNetUserId=@UserId");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.SpGetUserRole");
            DropStoredProcedure("dbo.SpDeleteUserRoleByUserId");
        }
    }
}
