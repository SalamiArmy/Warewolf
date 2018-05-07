﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.2.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Dev2.Activities.Specs.Sources
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.2.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class ServerSourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "ServerSource.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ServerSource", "\tIn order to create server source\r\n\tAs a Warewolf user\r\n\tI want to be able to use" +
                    " three authentication types", ProgrammingLanguage.CSharp, new string[] {
                        "ServerSourceTests"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "ServerSource")))
            {
                global::Dev2.Activities.Specs.Sources.ServerSourceFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(TestContext);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Create Windows Server Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ServerSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("ServerSourceTests")]
        public virtual void CreateWindowsServerSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Windows Server Source", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table896 = new TechTalk.SpecFlow.Table(new string[] {
                        "Address",
                        "AuthenticationType"});
            table896.AddRow(new string[] {
                        "http://tst-ci-remote:3142",
                        "Windows"});
#line 8
 testRunner.Given("I create a server source as", ((string)(null)), table896, "Given ");
#line 11
 testRunner.And("I save as \"WinServerSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.When("I Test \"WinServerSource\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
 testRunner.Then("The result is \"success\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 14
 testRunner.And("I delete serversource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Create User Server Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ServerSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("ServerSourceTests")]
        public virtual void CreateUserServerSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create User Server Source", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table897 = new TechTalk.SpecFlow.Table(new string[] {
                        "Address",
                        "AuthenticationType"});
            table897.AddRow(new string[] {
                        "http://tst-ci-remote:3142",
                        "User"});
#line 17
 testRunner.Given("I create a server source as", ((string)(null)), table897, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table898 = new TechTalk.SpecFlow.Table(new string[] {
                        "username",
                        "Password"});
            table898.AddRow(new string[] {
                        "dev2\\integrationtester",
                        "I73573r0"});
#line 20
 testRunner.And("User details as", ((string)(null)), table898, "And ");
#line 23
 testRunner.And("I save as \"WinServerSource1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.When("I Test \"WinServerSource1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
 testRunner.Then("The result is \"success\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 26
 testRunner.And("I delete serversource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Create Bad User Server Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ServerSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("ServerSourceTests")]
        public virtual void CreateBadUserServerSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Bad User Server Source", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table899 = new TechTalk.SpecFlow.Table(new string[] {
                        "Address",
                        "AuthenticationType"});
            table899.AddRow(new string[] {
                        "http://localhost:3142",
                        "User"});
#line 29
 testRunner.Given("I create a server source as", ((string)(null)), table899, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table900 = new TechTalk.SpecFlow.Table(new string[] {
                        "username",
                        "Password"});
            table900.AddRow(new string[] {
                        "BadUser",
                        "I73573r0"});
#line 32
 testRunner.And("User details as", ((string)(null)), table900, "And ");
#line 35
 testRunner.And("I save as \"WinServerSource2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.Then("The result is \"Connection Error :Unauthorized\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Create Public Server Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ServerSource")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("ServerSourceTests")]
        public virtual void CreatePublicServerSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Create Public Server Source", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table901 = new TechTalk.SpecFlow.Table(new string[] {
                        "Address",
                        "AuthenticationType"});
            table901.AddRow(new string[] {
                        "http://tst-ci-remote:3142",
                        "Public"});
#line 39
 testRunner.Given("I create a server source as", ((string)(null)), table901, "Given ");
#line 42
 testRunner.And("I save as \"WinServerSource2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
 testRunner.When("I Test \"WinServerSource2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 44
 testRunner.Then("The result is \"success\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 45
 testRunner.And("I delete serversource", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
