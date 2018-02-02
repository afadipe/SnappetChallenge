namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bill : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Staffs", new[] { "State_Id" });
            DropColumn("dbo.Staffs", "StateId");
            RenameColumn(table: "dbo.Staffs", name: "State_Id", newName: "StateId");
            CreateTable(
                "dbo.FeeItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProgrammeId = c.Long(nullable: false),
                        Name = c.String(),
                        FeeType = c.Int(nullable: false),
                        Option = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Instalmental = c.Double(nullable: false),
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
                "dbo.FeePaymentLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TransactionRef = c.String(maxLength: 50),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VatebraFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ResponseCode = c.String(maxLength: 20),
                        ResponseDescription = c.String(maxLength: 100),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        GetwayResponse = c.String(maxLength: 100),
                        PaymentReference = c.String(maxLength: 100),
                        RetrievalReferenceNumber = c.String(maxLength: 100),
                        TransactionCharges = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionRequest = c.String(maxLength: 500),
                        FeeItemId = c.Long(nullable: false),
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
                "dbo.LectureNotes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubjectID = c.Long(nullable: false),
                        ClassArm = c.Long(nullable: false),
                        SubmissionDate = c.DateTime(nullable: false),
                        AssignmentType = c.Int(nullable: false),
                        AssignmentText = c.String(),
                        FilePart = c.String(),
                        FileLocation = c.Byte(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DisplayName = c.String(),
                        Extension = c.String(),
                        Contenttype = c.String(),
                        FileData = c.Binary(),
                        StudentID = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        AssignmentID = c.Long(nullable: false),
                        Score = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                "dbo.StudentGradeAssignments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubjectID = c.Long(nullable: false),
                        ClassArm = c.Long(nullable: false),
                        SubmissionDate = c.DateTime(nullable: false),
                        AssignmentType = c.Int(nullable: false),
                        AssignmentText = c.String(),
                        FilePart = c.String(),
                        FileLocation = c.Byte(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        DisplayName = c.String(),
                        Extension = c.String(),
                        Contenttype = c.String(),
                        FileData = c.Binary(),
                        StudentID = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        AssignmentID = c.Long(nullable: false),
                        Score = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                "dbo.StudentBills",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FeeItemId = c.Long(nullable: false),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SessionId = c.Long(nullable: false),
                        TermId = c.Long(nullable: false),
                        ProgrammeId = c.Long(nullable: false),
                        ClassId = c.Long(nullable: false),
                        ArmId = c.Long(nullable: false),
                        PaymentStatus = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ClubMembers", "StudentID", c => c.Long(nullable: false));
            AddColumn("dbo.Staffs", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Staffs", "ReasonForTermination", c => c.Int());
            AddColumn("dbo.TechnologyFees", "RunPeriod", c => c.Int());
            AddColumn("dbo.TechnologyFees", "CreatedBy", c => c.Long(nullable: false));
            AddColumn("dbo.TechnologyFees", "UpdatedBy", c => c.Long(nullable: false));
            AddColumn("dbo.TechnologyFees", "DateUpdated", c => c.DateTime());
            AddColumn("dbo.TechnologyFees", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("dbo.ClubMembers", "ClassID", c => c.Long(nullable: false));
            AlterColumn("dbo.ClubMembers", "ArmID", c => c.Long(nullable: false));
            AlterColumn("dbo.ClubMembers", "SessionID", c => c.Long());
            AlterColumn("dbo.ClubMembers", "TermID", c => c.Long());
            AlterColumn("dbo.Staffs", "StateId", c => c.Long());
            AlterColumn("dbo.Students", "Gender", c => c.Int(nullable: false));
            CreateIndex("dbo.Staffs", "StateId");
            DropColumn("dbo.ClubMembers", "StudentName");
            DropColumn("dbo.TechnologyFees", "AutoGenerateOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TechnologyFees", "AutoGenerateOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.ClubMembers", "StudentName", c => c.String());
            DropIndex("dbo.Staffs", new[] { "StateId" });
            AlterColumn("dbo.Students", "Gender", c => c.Long(nullable: false));
            AlterColumn("dbo.Staffs", "StateId", c => c.Int());
            AlterColumn("dbo.ClubMembers", "TermID", c => c.Long(nullable: false));
            AlterColumn("dbo.ClubMembers", "SessionID", c => c.Long(nullable: false));
            AlterColumn("dbo.ClubMembers", "ArmID", c => c.String());
            AlterColumn("dbo.ClubMembers", "ClassID", c => c.String());
            DropColumn("dbo.TechnologyFees", "RowVersion");
            DropColumn("dbo.TechnologyFees", "DateUpdated");
            DropColumn("dbo.TechnologyFees", "UpdatedBy");
            DropColumn("dbo.TechnologyFees", "CreatedBy");
            DropColumn("dbo.TechnologyFees", "RunPeriod");
            DropColumn("dbo.Staffs", "ReasonForTermination");
            DropColumn("dbo.Staffs", "Gender");
            DropColumn("dbo.ClubMembers", "StudentID");
            DropTable("dbo.StudentBills");
            DropTable("dbo.StudentGradeAssignments");
            DropTable("dbo.LectureNotes");
            DropTable("dbo.FeePaymentLogs");
            DropTable("dbo.FeeItems");
            RenameColumn(table: "dbo.Staffs", name: "StateId", newName: "State_Id");
            AddColumn("dbo.Staffs", "StateId", c => c.Int());
            CreateIndex("dbo.Staffs", "State_Id");
        }
    }
}
