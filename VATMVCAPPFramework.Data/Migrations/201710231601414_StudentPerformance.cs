namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentPerformance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Performances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StudentId = c.Long(nullable: false),
                        UniqueComment = c.String(),
                        PrincipalComment = c.String(),
                        ArmId = c.Long(nullable: false),
                        TermId = c.Long(nullable: false),
                        SessionId = c.Long(nullable: false),
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
            DropTable("dbo.Performances");
        }
    }
}
