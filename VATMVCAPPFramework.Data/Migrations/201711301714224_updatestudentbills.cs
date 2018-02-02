namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatestudentbills : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StudentBills", "StudentID", c => c.Int(nullable: false));
            AlterColumn("dbo.StudentBills", "StudentId", c => c.Long(nullable: false));
            DropTable("dbo.RoomDealocationHistories");
            DropTable("dbo.RoomRoundCheckListSettings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoomRoundCheckListSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoomDealocationHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HallID = c.Long(nullable: false),
                        RoomID = c.Long(nullable: false),
                        BedID = c.Long(nullable: false),
                        StudentID = c.Long(nullable: false),
                        ExpiredDate = c.DateTime(nullable: false),
                        SessionID = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.StudentBills", "StudentId", c => c.Int(nullable: false));
            AlterColumn("dbo.StudentBills", "StudentID", c => c.Long(nullable: false));
        }
    }
}
