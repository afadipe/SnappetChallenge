namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatestUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentParents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ParentId = c.Long(nullable: false),
                        StudentId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StudentParents");
        }
    }
}
