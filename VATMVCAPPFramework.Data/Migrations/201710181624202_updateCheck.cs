namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCheck : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TechnologyFees", "RunPeriod", c => c.Int(nullable: false));
            DropTable("dbo.TechnologyFeeRunPeriods");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TechnologyFeeRunPeriods",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TechnologyInvoiceId = c.Long(nullable: false),
                        LastRunDate = c.DateTime(nullable: false),
                        NextRunDate = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.TechnologyFees", "RunPeriod", c => c.Int());
        }
    }
}
