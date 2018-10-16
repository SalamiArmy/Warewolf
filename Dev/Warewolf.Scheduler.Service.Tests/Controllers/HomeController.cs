using Hangfire;
using System;
using System.Web.Mvc;
using Warewolf.Scheduler.Service.Tests.Models;

namespace Warewolf.Scheduler.Service.Tests.Controllers
{
    public class HomeController
    {
        private readonly IBackgroundJobClient _jobClient;

        public HomeController() : this(new BackgroundJobClient()) { }

        public HomeController(IBackgroundJobClient jobClient)
        {
            _jobClient = jobClient;
        }

        public ActionResult Create(Comment comment)
        {
            _jobClient.Enqueue(() => CheckForSpam());
            return null;
        }

        public void CheckForSpam()
        {
            Console.WriteLine("CheckForSpam");
        }

        public void WriteToConsole(string message)
        {
            Console.WriteLine(message);
        }
        public ActionResult Index()
        {
            _jobClient.Enqueue(() => WriteToConsole("Background Job completed successfully!"));
            return null;
        }
    }
}
