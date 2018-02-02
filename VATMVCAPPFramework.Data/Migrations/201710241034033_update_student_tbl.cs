namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_student_tbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "Gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "Gender", c => c.Long(nullable: false));
        }
    }
}
