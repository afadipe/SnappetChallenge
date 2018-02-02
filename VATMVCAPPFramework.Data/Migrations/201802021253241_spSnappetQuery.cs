namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spSnappetQuery : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("dbo.spGetAssignmentByUserIdBI",
                   body:
                   @"SELECT c.UserId Name , count(c.Id) as count
                    FROM  StudentPerformances c 
                    group by c.UserId
                    order by c.UserId
              ");


            CreateStoredProcedure("dbo.spGetAssignmentByStudentBI",
                  body:
                  @"SELECT c.Subject Name , count(c.Id) as count
                    FROM  StudentPerformances c 
                    group by c.Subject");


            CreateStoredProcedure("dbo.spStudnetCountPerAssignmentBI",
                body:
                @"SELECT c.UserId Name , count(c.Id) as count
                    FROM  StudentPerformances c 
                    group by c.UserId
                    order by c.UserId");

        }
        
        public override void Down()
        {
        }
    }
}
