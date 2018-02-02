using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VATMVCAPPFramework.HangfireJob;
using VATMVCAPPFramework.HangfireSetting;

namespace VATMVCAPPFramework
{
    public class HangfireConfig
    {

        public static void ConfigureHangfire(IAppBuilder app)
        {
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection").UseFilter(new HangfireLoggerAttribute());

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangireDashboardAuthorizationFilter() }
            });

            //var container = new Container();
            //GlobalConfiguration.Configuration.UseActivator(new ContainerJobActivator(container));

            app.UseHangfireServer();
            GlobalJobFilters.Filters.Add(new HangfireLoggerAttribute());
        }
        public static void InitializeJobs()
        {
            RecurringJob.AddOrUpdate<HangfireEmailJob>(j => j.Execute(), Cron.Minutely);
        }
    }
}