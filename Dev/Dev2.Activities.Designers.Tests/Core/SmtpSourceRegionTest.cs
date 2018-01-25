using Dev2.Activities.Designers2.Core.Source;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.ToolBase.Email;
using Dev2.Studio.Core.Activities.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.ObjectModel;

namespace Dev2.Activities.Designers.Tests.Core
{
    [TestClass]
    public class SmtpSourceRegionTest
    {
        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SmtpSourceRegion_Constructor")]
        public void SmtpSourceRegion_Constructor_Scenerio_Result()
        {
            //------------Setup for test--------------------------
            var src = new Mock<ISmtpServiceModel>();
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<ISmtpSource>());

            //------------Execute Test---------------------------
            var region = new SmtpSourceRegion(src.Object, ModelItemUtils.CreateModelItem(new DsfSendEmailActivity()), "EmailSource");

            //------------Assert Results-------------------------
            Assert.IsNull(region.Errors);
            Assert.IsTrue(region.IsEnabled);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SmtpSourceRegion_ConstructorWithSelectedSource")]
        public void SmtpSourceRegion_ConstructorWithSelectedSource_Scenerio_Result()
        {
            //------------Setup for test--------------------------
            var id = Guid.NewGuid();
            var emailSourceDef = new EmailServiceSourceDefinition { Id = id };
            var act = new DsfSendEmailActivity { SavedSource = emailSourceDef };
            var src = new Mock<ISmtpServiceModel>();
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<ISmtpSource> { emailSourceDef });

            //------------Execute Test---------------------------
            var region = new SmtpSourceRegion(src.Object, ModelItemUtils.CreateModelItem(act), "EmailSource")
            {
                SelectedSource = emailSourceDef
            };
            //------------Assert Results-------------------------
            Assert.AreEqual(emailSourceDef, region.SelectedSource);
            Assert.IsTrue(region.CanEditSource());
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SmtpSourceRegion_ChangeSourceSomethingChanged")]
        public void ComSourceRegion_ChangeSourceSomethingChanged_ExpectedChange_Result()
        {
            //------------Setup for test--------------------------
            var id = Guid.NewGuid();
            var emailSourceDef = new EmailServiceSourceDefinition { Id = id };
            var act = new DsfSendEmailActivity { SavedSource = emailSourceDef };
            var src = new Mock<ISmtpServiceModel>();
            var evt = false;
            var emailSourceDef2 = new EmailServiceSourceDefinition { Id = Guid.NewGuid() };
            src.Setup(a => a.RetrieveSources()).Returns(new ObservableCollection<ISmtpSource> { emailSourceDef, emailSourceDef2 });

            //------------Execute Test---------------------------
            var region = new SmtpSourceRegion(src.Object, ModelItemUtils.CreateModelItem(act), "EmailSource");
            region.SomethingChanged += (a, b) => { evt = true; };
            region.SelectedSource = emailSourceDef2;

            //------------Assert Results-------------------------
            Assert.IsTrue(evt);
        }
    }
}
