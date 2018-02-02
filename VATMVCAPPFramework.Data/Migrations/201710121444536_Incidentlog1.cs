namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Incidentlog1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncidentLog", "UserId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IncidentLog", "UserId");
        }
    }
}
