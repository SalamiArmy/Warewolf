using System.Data.Entity;

namespace Warewolf.Scheduler.Service.Models
{
    public class SchedulerDbContext : DbContext
    {
        public SchedulerDbContext() : base("SchedulerDb")
        {
        }
        public DbSet<Scheduler> Scheduler { get; set; }
    }
}