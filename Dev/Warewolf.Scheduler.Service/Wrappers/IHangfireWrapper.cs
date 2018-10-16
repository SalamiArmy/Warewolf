using Hangfire;

namespace Warewolf.Scheduler.Service
{
    public interface IHangfireWrapper
    {
        IBackgroundJobClient BackgroundJobClient { get; }
    }
}
