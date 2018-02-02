namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_student_perf_tbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PasswordHistory", "UpdatedBy", c => c.Long());
            DropColumn("dbo.AspNetUsers", "DOB");
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "DeletedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DeletedBy", c => c.Long());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "DOB", c => c.DateTime());
            AlterColumn("dbo.PasswordHistory", "UpdatedBy", c => c.Long(nullable: false));
        }
    }
}
