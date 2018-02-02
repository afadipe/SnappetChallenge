namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Languagetbl2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DOB", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            DropTable("dbo.ProgrammeSessionTerms");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProgrammeSessionTerms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SessionID = c.Long(nullable: false),
                        ProgrammeID = c.Long(nullable: false),
                        TermID = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "DOB");
        }
    }
}
