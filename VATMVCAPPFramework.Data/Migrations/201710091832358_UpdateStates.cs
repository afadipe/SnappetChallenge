namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.States", "NationalityID", c => c.Long(nullable: false));
            AlterColumn("dbo.States", "NationalityID", c => c.Long(nullable: true));
            DropColumn("dbo.States", "LGAId");
        }
        
        public override void Down()
        {
            DropColumn("dbo.States", "NationalityID");
        }
    }
}
