namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FetchSubjectSelection : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("dbo.FetchClassSubjectSelection",
                x => new {
                    ProgrammeId = x.Int(0),
                    ClassId = x.Int(0),
                    ArmId = x.Int(0)
                },
              body:
              @"select  ID = Isnull(f.Id,0), ArmID = a.Id
    		    ,ClassID =b.Id
    		    ,ProgrammeID = c.Id
				,Isnull(SubjectID,0) 
    	        ,b.Name Class
    		    ,c.Name Programme
    		    ,a.Name Arm
    		    ,Isnull(Count(f.[SubjectID]),0) NoOfSubjects,
    				TeacherID =f.TeacherId
    		    from Arms a 
    		    left join Classes b on a.ClassID =b.Id
    		    left join [Programmes] c on a.[ProgrammeID]=c.Id
    		    left join [ClassSubjects] f on a.Id = f.ArmId and b.Id = a.ClassId and c.Id = a.ProgrammeID
				Where (@ProgrammeId = 0 OR @ProgrammeId = c.Id) and
				(@ClassId = 0 OR @ClassId = b.Id) and
				(@ArmId =0 OR @ArmId = a.Id)

    		    Group by c.[Id],b.[Id],a.[Id],c.Name,b.Name,a.Name,f.Id,f.SubjectID,f.TeacherID
    		    order by a.Id");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.FetchClassSubjectSelection");
        }
    }
}
