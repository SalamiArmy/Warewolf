using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Warewolf.Scheduler.Service.Tests.Controllers;
using Warewolf.Scheduler.Service.Tests.Models;

namespace Warewolf.Scheduler.Service.Tests
{
    [TestClass]
    public class TestSchedulerService
    {
        [TestMethod]
        public void CreateAction_ShouldEnqueueAJob()
        {
            var jobClient = new Mock<IBackgroundJobClient>();
            jobClient.Setup(c => c.Create(It.IsAny<Job>(), It.IsAny<EnqueuedState>()));

            var controller = new HomeController(jobClient.Object);
            var comment = new Comment();
            controller.Create(comment);

            jobClient.Verify(x => x.Create(It.Is<Job>(job => job.Method.Name == "CheckForSpam"), It.IsAny<EnqueuedState>()));
        }
    }   
}
