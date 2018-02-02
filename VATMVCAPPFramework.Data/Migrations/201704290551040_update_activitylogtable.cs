namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_activitylogtable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActivityLog", "UserId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActivityLog", "UserId", c => c.Int());
        }
    }
}
