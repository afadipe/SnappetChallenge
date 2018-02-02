namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tblupdatebills : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StudentBills", "StudentId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StudentBills", "StudentId", c => c.Int(nullable: false));
        }
    }
}
