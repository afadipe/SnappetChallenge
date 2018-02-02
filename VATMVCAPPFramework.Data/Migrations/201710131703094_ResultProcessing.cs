namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResultProcessing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Disengages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReasonType = c.Long(nullable: false),
                        Detail = c.String(),
                        UserId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DisengageType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DisengageTypes", t => t.DisengageType_Id)
                .Index(t => t.DisengageType_Id);
            
            CreateTable(
                "dbo.DisengageTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GradingSystems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProgrammeID = c.Long(nullable: false),
                        MinScore = c.Int(nullable: false),
                        MaxScore = c.Int(nullable: false),
                        Grade = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ScoreGradeEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ArmId = c.Long(nullable: false),
                        SubjectId = c.Long(nullable: false),
                        StudentId = c.Long(nullable: false),
                        Score = c.String(),
                        GradeId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
                
            
            CreateTable(
                "dbo.Suspensions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Detail = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        UserId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScoreGradeEntries", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ScoreGradeEntries", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ScoreGradeEntries", "ArmId", "dbo.Arms");
            DropForeignKey("dbo.Disengages", "DisengageType_Id", "dbo.DisengageTypes");
            DropIndex("dbo.ScoreGradeEntries", new[] { "StudentId" });
            DropIndex("dbo.ScoreGradeEntries", new[] { "SubjectId" });
            DropIndex("dbo.ScoreGradeEntries", new[] { "ArmId" });
            DropIndex("dbo.Disengages", new[] { "DisengageType_Id" });
            DropTable("dbo.Suspensions");
            DropTable("dbo.ScoreGradeEntries");
            DropTable("dbo.GradingSystems");
            DropTable("dbo.DisengageTypes");
            DropTable("dbo.Disengages");
        }
    }
}
