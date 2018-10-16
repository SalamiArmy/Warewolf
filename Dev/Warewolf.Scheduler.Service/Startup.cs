using Hangfire;
using Microsoft.Owin;
using Owin;
using System;

namespace Warewolf.Scheduler.Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("SchedulerDb");
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting Started with HangFire!"));
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
