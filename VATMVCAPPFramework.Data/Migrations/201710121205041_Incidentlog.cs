namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Incidentlog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IncidentCategory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncidentLog",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        CategoryType = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                        IncidentCategory_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IncidentCategory", t => t.IncidentCategory_Id)
                .Index(t => t.IncidentCategory_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncidentLog", "IncidentCategory_Id", "dbo.IncidentCategory");
            DropIndex("dbo.IncidentLog", new[] { "IncidentCategory_Id" });
            DropTable("dbo.IncidentLog");
            DropTable("dbo.IncidentCategory");
        }
    }
}
