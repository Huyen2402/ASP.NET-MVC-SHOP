using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Management;
using Hangfire;
using Hangfire.SqlServer;
using WebsiteBanHang.Controllers;

[assembly: OwinStartup(typeof(WebsiteBanHang.Startup))]

namespace WebsiteBanHang
{
    public class Startup
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage("Server=localhost; Database=HangfireTest; Integrated Security=True;", new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            app.UseHangfireAspNet(GetHangfireServers);
            //app.UseHangfireDashboard();
            LienHeController obj = new LienHeController();
            RecurringJob.AddOrUpdate(() => obj.RestartPrice(), Cron.Daily);
            DashboardOptions opts = new DashboardOptions
            {
                AuthorizationFilters = new[] { new AuthorisationOverride() }
            };
            app.UseHangfireDashboard("/myJobDashboard", opts);

            app.UseHangfireServer();
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888


            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }



    }
    public class AuthorisationOverride : Hangfire.Dashboard.IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            return true;
        }
    }
}
