namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssessmentTypeId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScoreGradeEntries", "AssessmentType", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScoreGradeEntries", "AssessmentType");
        }
    }
}
