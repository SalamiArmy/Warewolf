

using Hangfire;
using System;

namespace Warewolf.Scheduler.Service
{
    public class Scheduler
    {
        public void FireAndForget(IHangfireWrapper hangfireWrapper)
        {
            var jobId = hangfireWrapper.BackgroundJobClient.Schedule(() => Console.WriteLine("Delayed!"), TimeSpan.FromDays(7));
        }

        public void Delayed(IHangfireWrapper hangfireWrapper)
        {

            var jobId = hangfireWrapper.BackgroundJobClient.Schedule(() => Console.WriteLine("Delayed!"), System.TimeSpan.FromDays(7));

        }

        public void Recurring(IHangfireWrapper hangfireWrapper)
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring!"), Cron.Daily);
        }

        //public void Continuations(IHangfireWrapper hangfireWrapper, int jobId)
        //{
        //    BackgroundJob.ContinueWith(jobId, () => Console.WriteLine("ContinueWith!"));
        //}
    }
}