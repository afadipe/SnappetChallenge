namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_stuentper_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentPerformances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubmittedAnswerId = c.Int(nullable: false),
                        SubmitDateTime = c.DateTime(nullable: false),
                        Correct = c.Int(nullable: false),
                        Progress = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ExerciseId = c.Int(nullable: false),
                        Difficulty = c.String(),
                        Subject = c.String(),
                        Domain = c.String(),
                        LearningObjective = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StudentPerformances");
        }
    }
}
