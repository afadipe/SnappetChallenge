namespace VATMVCAPPFramework.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VATMVCAPPFramework.Data.Entities;
    using VATMVCAPPFramework.Data.IdentityModel;

    internal sealed class Configuration : DbMigrationsConfiguration<VATMVCAPPFramework.Data.APPContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(VATMVCAPPFramework.Data.APPContext context)
        {
            
            //  This method will be called after migrating to the latest version.
            context.ApplicationRoles.AddOrUpdate(p => p.Name, new ApplicationRole { Name = "PortalAdmin" });
          

            context.PortalVersions.AddOrUpdate(p => p.FrameworkName,
                new PortalVersion
                {
                    FrameworkName = "VATMVCFramework",
                    FrameworkDescription = "An MVC Customized Framework built on ASP.Net Identity 2.0 to aid fast application development with built in logger and activitylog",
                    FrameworkVersion = "1.17.0.0",
                    TargetServer = "MSSQL,Postgress,MangoDB,MYSQL",
                    PackagesUsed = "Microsoft.ASPNET.Identity,Microsoft.OWIN,Log4net,EntityFramework,JQuery DataTable,Select 2,Boostrap,ORM(Dapper,EntityFramework),Autofac,Autofac.MVC,Autofac.WebAPI2,CQRS RepositoryPattern",
                    DefaultDatabaseEngine = "MSSQL Server",
                    IOC = "Autofac",
                    DateCreated = DateTime.Now,
                    DevelopedBy = "Fadipe Ayobami  || ayfadipe@gmail.com",
                    UX = "Open SOurce"
                });

            base.Seed(context);


        }
    }
}
