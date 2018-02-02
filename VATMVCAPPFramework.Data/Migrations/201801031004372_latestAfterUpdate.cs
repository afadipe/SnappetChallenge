namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class latestAfterUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Campus", "BankId", c => c.Long(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Campus", "BankId", c => c.Long());
        }
    }
}
