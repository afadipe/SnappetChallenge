namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResultprocessingUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScoreGradeEntries", "ClassId", c => c.Long(nullable: false));
            AddColumn("dbo.ScoreGradeEntries", "SessionId", c => c.Long(nullable: false));
            AddColumn("dbo.ScoreGradeEntries", "TermId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScoreGradeEntries", "TermId");
            DropColumn("dbo.ScoreGradeEntries", "SessionId");
            DropColumn("dbo.ScoreGradeEntries", "ClassId");
        }
    }
}
