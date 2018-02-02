namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FetchClassSubjectSelection : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("dbo.FetchClassSubjectSelection",
            body:
            @"select  ArmId = a.Id
		    ,ClassID =b.Id
		    ,ProgrammeID = c.Id 
      	    ,b.Name Class
		    ,c.Name Programme
		    ,a.Name Arm
		    ,Isnull(Count(f.[SubjectID]),0) NoOfSubjects
		    from Arms a 
		    left join Classes b on a.ClassID =b.Id
		    left join [Programmes] c on a.[ProgrammeID]=c.Id
		    left join [ClassSubjects] f on a.Id = f.ArmId and b.Id = a.ClassId and c.Id = a.ProgrammeID
    
		    Group by c.[Id],b.[Id],a.[Id],c.Name,b.Name,a.Name
		    order by a.Id");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FetchClassSubjectSelection");
        }
    }
}
