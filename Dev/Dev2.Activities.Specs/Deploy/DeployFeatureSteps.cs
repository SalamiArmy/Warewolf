﻿using Dev2.Activities.Specs.BaseTypes;
using Dev2.Studio.Core;
using Dev2.Util;
using System;
using System.Collections.Generic;
using Dev2.Data.ServiceModel;
using Dev2.Network;
using Dev2.Studio.Core.Models;
using Dev2.Studio.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using System.Linq;
using System.IO;

namespace Dev2.Activities.Specs.Deploy
{
    [Binding]
    public sealed class DeployFeatureSteps
    {
        static ScenarioContext _scenarioContext;
        readonly CommonSteps _commonSteps;
        Guid _resourceId = Guid.Parse("fbc83b75-194a-4b10-b50c-b548dd20b408");

        public DeployFeatureSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext ?? throw new ArgumentNullException("scenarioContext");
            _commonSteps = new CommonSteps(_scenarioContext);
        }
        
        [BeforeScenario("Deploy")]
        public void RollBack()
        {
            var formattableString = $"http://tst-ci-remote:3142";
            AppUsageStats.LocalHost = $"http://{Environment.MachineName}:3142";
            IServer remoteServer = new Server(new Guid(), new ServerProxy(new Uri(formattableString)))
            {
                Name = "tst-ci-remote"
            };
            ScenarioContext.Current.Add("destinationServer", remoteServer);
            var previousVersions = remoteServer.ProxyLayer.GetVersions(_resourceId);
            if (previousVersions != null && previousVersions.Count > 0)
            {
                remoteServer.ProxyLayer.Rollback(_resourceId, previousVersions.First().VersionNumber);
            }
        }

        [Given(@"I am Connected to remote server ""(.*)""")]
        public void GivenIAmConnectedToServer(string connectinName)
        {
            var localhost = ServerRepository.Instance.Source;
            ScenarioContext.Current.Add("sourceServer", localhost);
            localhost.Connect();
            var remoteServer = ScenarioContext.Current.Get<IServer>("destinationServer");
            remoteServer.Connect();
        }

        [Then(@"And the destination resource is ""(.*)""")]
        public void ThenAndTheLocalhostResourceIs(string p0)
        {
            var remoteServer = ScenarioContext.Current.Get<IServer>("destinationServer");
            var loadContextualResourceModel = remoteServer.ResourceRepository.LoadContextualResourceModel(_resourceId);
            Assert.AreEqual(p0, loadContextualResourceModel.DisplayName, "Expected Resource to be " + p0 + " on load for ci-remote");
        }

        //[Given(@"I select resource ""(.*)"" from source server")]
        //[When(@"I select resource ""(.*)"" from source server")]
        //[Then(@"I select resource ""(.*)"" from source server")]
        //public void GivenISelectResourceFromSourceServer(string workflowName)
        //{
        //    //TryGetValue(workflowName, out IContextualResourceModel resourceModel);
        //    var localhost = ScenarioContext.Current.Get<IServer>("sourceServer");            
        //    var loadContextualResourceModel = localhost.ResourceRepository.LoadContextualResourceModel(_resourceId);
        //    Assert.IsNotNull(loadContextualResourceModel, workflowName + "does not exist on the local machine " + Environment.MachineName);
        //    ScenarioContext.Current.Add("localResource", loadContextualResourceModel);
        //}

        [Given(@"And the localhost resource is ""(.*)""")]
        public void GivenAndTheLocalhostResourceIs(string p0)
        {
            var loaclHost = ScenarioContext.Current.Get<IServer>("sourceServer");
            var loadContextualResourceModel = loaclHost.ResourceRepository.LoadContextualResourceModel(_resourceId);
            Assert.AreEqual(p0, loadContextualResourceModel.DisplayName, "Expected Resource to be " + p0 + " on load for localhost");
            Assert.AreEqual(p0, loadContextualResourceModel.ResourceName, "Expected Resource to be " + p0 + " on load for localhost");
        }

       

        [Given(@"I reload the destination resources")]
        [When(@"I reload the destination resources")]
        [Then(@"I reload the destination resources")]
        public void WhenIReloadTheRemoteServerResources()
        {
            var remoteServer = ScenarioContext.Current.Get<IServer>("destinationServer");
            var loadContextualResourceModel = remoteServer.ResourceRepository.LoadContextualResourceModel(_resourceId);
            ScenarioContext.Current["serverResource"] = loadContextualResourceModel;
        }
        [Given(@"I reload the source resources")]
        [When(@"I reload the source resources")]
        [Then(@"I reload the source resources")]
        public void WhenIReloadTheSourceResources()
        {
            var localhost = ScenarioContext.Current.Get<IServer>("sourceServer");
            localhost.ResourceRepository.ForceLoad();
        }

        [Then(@"the destination resource is ""(.*)""")]
        [Given(@"the destination resource is ""(.*)""")]
        [When(@"the destination resource is ""(.*)""")]
        public void ThenDestinationResourceIs(string p0)
        {
            var destinationServer = ScenarioContext.Current.Get<IServer>("destinationServer");
            var loadContextualResourceModel = destinationServer.ResourceRepository.LoadContextualResourceModel(_resourceId);
            Assert.AreEqual(p0, loadContextualResourceModel.DisplayName, "Failed to Update " + loadContextualResourceModel.DisplayName + " after deploy");
            Assert.AreEqual(p0, loadContextualResourceModel.ResourceName, "Failed to Update " + loadContextualResourceModel.ResourceName + " after deploy");
        }


        [Given(@"I RollBack Resource")]
        [When(@"I RollBack Resource")]
        [Then(@"I RollBack Resource")]
        public void RollBackResource()
        {
            var destinationServer = ScenarioContext.Current.Get<IServer>("destinationServer");
            var previousVersions = destinationServer.ProxyLayer.GetVersions(_resourceId);
            destinationServer.ProxyLayer.Rollback(_resourceId, previousVersions.First().VersionNumber);
        }
    }
}
