namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArmId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassSubjects", "ArmId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassSubjects", "ArmId");
        }
    }
}
