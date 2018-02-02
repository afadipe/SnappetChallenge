namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_email_tbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EmailLogs", "CC", c => c.String(maxLength: 1000));
            AlterColumn("dbo.EmailLogs", "BCC", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmailLogs", "BCC", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.EmailLogs", "CC", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
