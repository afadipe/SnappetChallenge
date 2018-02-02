namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_email_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailAttachments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmailLogID = c.Long(nullable: false),
                        FilePath = c.String(nullable: false),
                        FileTitle = c.String(maxLength: 50),
                        DateCreated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmailLogs", t => t.EmailLogID, cascadeDelete: true)
                .Index(t => t.EmailLogID);
            
            CreateTable(
                "dbo.EmailLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Sender = c.String(nullable: false, maxLength: 1000),
                        Receiver = c.String(nullable: false, maxLength: 1000),
                        CC = c.String(nullable: false, maxLength: 1000),
                        BCC = c.String(nullable: false, maxLength: 1000),
                        Subject = c.String(nullable: false, maxLength: 1000),
                        MessageBody = c.String(nullable: false),
                        HasAttachment = c.Boolean(nullable: false),
                        IsSent = c.Boolean(nullable: false),
                        Retires = c.Int(nullable: false),
                        DateSent = c.DateTime(),
                        DateToSend = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        Body = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTokens",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EmailCode = c.String(),
                        Token = c.String(),
                        PreviewText = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailAttachments", "EmailLogID", "dbo.EmailLogs");
            DropIndex("dbo.EmailAttachments", new[] { "EmailLogID" });
            DropTable("dbo.EmailTokens");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailLogs");
            DropTable("dbo.EmailAttachments");
        }
    }
}
