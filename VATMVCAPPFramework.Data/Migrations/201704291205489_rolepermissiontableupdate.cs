namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rolepermissiontableupdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RolePermission", "PermissionId", c => c.Long(nullable: false));
            AlterColumn("dbo.RolePermission", "RoleId", c => c.Long(nullable: false));

            CreateStoredProcedure("dbo.DeletePermissionByRoleID",
                p => new
                {
                    RoleId = p.Int()
                },
                body:
                @"delete from RolePermission where RoleId= @RoleId"
            );
        }
        
        public override void Down()
        {
           
            AlterColumn("dbo.RolePermission", "RoleId", c => c.Int(nullable: false));
            AlterColumn("dbo.RolePermission", "PermissionId", c => c.Int(nullable: false));
            DropStoredProcedure("dbo.DeletePermissionByRoleID");
        }
    }
}
