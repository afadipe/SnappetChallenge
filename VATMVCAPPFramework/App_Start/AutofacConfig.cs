using System;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using VATMVCAPPFramework.Data;
using VATMVCAPPFramework.Repository.CoreRepositories;
using VATMVCAPPFramework.Repository;
using VATMVCAPPFramework.Data.LibraryContainer.LoggingService;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

namespace VATMVCAPPFramework
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            // Setup the Container Builder
            var builder = new ContainerBuilder();
            // Register the controller in scope 
            //Register  WVC controllers all at once using assembly scanning...
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            //Register webAPI Controller
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //Registering the email context
           // builder.RegisterType<EmailEngineModel>().InstancePerRequest();

            // OPTIONAL: Register the Autofac filter provider.
            //builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // ...or you can register individual controlllers manually.
            //builder.RegisterType<ValuesController>().InstancePerRequest();

            builder.RegisterType<APPContext>().InstancePerRequest();
            builder.RegisterType<Random>().InstancePerRequest();


            builder.RegisterGeneric(typeof(RepositoryQuery<>))
               .As(typeof(IRepositoryQuery<>))
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(RepositoryCommand<>))
              .As(typeof(IRepositoryCommand<>))
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(IAutoDependencyRegister).Assembly)
               .AssignableTo<IAutoDependencyRegister>()
               .As<IAutoDependencyRegister>()
               .AsImplementedInterfaces().InstancePerRequest();

            


            builder.RegisterType<VATMVCAPPFramework.Data.IdentityService.ApplicationRoleStore>().As<IRoleStore<VATMVCAPPFramework.Data.IdentityModel.ApplicationRole,Int64>>().InstancePerRequest();
            builder.RegisterType<VATMVCAPPFramework.Data.IdentityService.ApplicationUserStore>().As<IUserStore<VATMVCAPPFramework.Data.IdentityModel.ApplicationUser,Int64>>().InstancePerRequest();
            builder.RegisterType<VATMVCAPPFramework.Data.IdentityService.ApplicationRoleManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<VATMVCAPPFramework.Data.IdentityService.ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<VATMVCAPPFramework.Data.IdentityService.ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.Register(c =>HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>().InstancePerRequest();
            builder.RegisterType<Utilities.Utility>().AsSelf().InstancePerRequest();

            //Add the Logging module
            builder.RegisterModule(new LoggerModule());
            //Add filter module
            builder.RegisterFilterProvider();
            // Build the container
            var container = builder.Build();

            // Setup the dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);


        }
    }
}