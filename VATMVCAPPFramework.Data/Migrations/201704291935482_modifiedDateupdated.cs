namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifiedDateupdated : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Permission", "DateUpdated", c => c.DateTime());
            AlterColumn("dbo.RolePermission", "DateUpdated", c => c.DateTime());

            CreateStoredProcedure("dbo.SpGetActivityLog",
              p => new
              {
                  UserId = p.Int(0),
                  ControllerName=p.String(null),
                  StartDate = p.DateTime(null),
                  EndDate = p.DateTime(null)
              },
              body:
              @"select  isnull(e.FirstName + ' ' + e.MiddleName + ' ' + e.LastName,'N/A') FullName,
	              a.Id,
                  a.[UserID]
                  ,a.[Description]
                  ,a.[DateCreated]
                  ,a.[ModuleName]
                  ,a.[ModuleAction]
                  ,isnull(a.[Record],'N/A') Record
              FROM [dbo].[ActivityLog] a
              left outer join [dbo].[AspNetUsers] e on  a.[UserID]=e.AspNetUserId
              where (@UserId=0 or e.AspNetUserId=@UserId)
              and (@ControllerName='' or a.ModuleName=@ControllerName)
              and  
                (a.DateCreated >= @StartDate OR @StartDate is null)
                AND
                (a.DateCreated  <= @EndDate OR  @EndDate is null)
              order by a.Id desc");


            CreateStoredProcedure("dbo.SpGetAllActivityLog",
              body:
              @"select  isnull(e.FirstName + ' ' + e.MiddleName + ' ' + e.LastName,'N/A') FullName,
	            a.Id,
                a.[UserID]
                ,a.[Description]
                ,a.[DateCreated]
                ,a.[ModuleName]
                ,a.[ModuleAction]
                ,isnull(a.[Record],'N/A') Record
            FROM [dbo].[ActivityLog] a
            left outer join [dbo].[AspNetUsers] e on  a.[UserID]=e.AspNetUserId
            order by a.Id desc");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RolePermission", "DateUpdated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Permission", "DateUpdated", c => c.DateTime(nullable: false));
            DropStoredProcedure("dbo.SpGetActivityLog");
            DropStoredProcedure("dbo.SpGetAllActivityLog");
            
        }
    }
}
