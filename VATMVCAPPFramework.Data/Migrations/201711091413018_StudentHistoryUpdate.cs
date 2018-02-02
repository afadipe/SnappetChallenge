namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentHistoryUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentHistories", "ProgrammeId", c => c.Long());
            AddColumn("dbo.StudentHistories", "ClassId", c => c.Long());
            AddColumn("dbo.StudentHistories", "ArmId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentHistories", "ArmId");
            DropColumn("dbo.StudentHistories", "ClassId");
            DropColumn("dbo.StudentHistories", "ProgrammeId");
        }
    }
}
