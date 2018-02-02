namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scoregrade : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ScoreGradeEntries", "AssessmentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ScoreGradeEntries", "AssessmentType", c => c.Long(nullable: false));
        }
    }
}
