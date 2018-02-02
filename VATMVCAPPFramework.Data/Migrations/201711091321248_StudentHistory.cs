namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        SessionId = c.Long(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Students", "SessionId", c => c.Long());
            DropColumn("dbo.Students", "EducationPeriodId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "EducationPeriodId", c => c.Long());
            DropColumn("dbo.Students", "SessionId");
            DropTable("dbo.StudentHistories");
        }
    }
}
