using System;
using System.ComponentModel.DataAnnotations;

namespace Warewolf.Scheduler.Service.Models
{
    public class Scheduler
    {
        public int JobId { get; set; }

        public string JobName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ScheduleAt { get; set; }
    }
}