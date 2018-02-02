namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateScoretable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScoreGradeEntries", "TotalScore", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScoreGradeEntries", "TotalScore");
        }
    }
}
