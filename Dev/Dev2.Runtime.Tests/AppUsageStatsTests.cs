using System;
using Dev2.Common;
using Dev2.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dev2.Tests.Runtime
{
    [TestClass]
    public class AppUsageStatsTests
    {
        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory("RevulyticsCollectUsageStats")]
        [DoNotParallelize]
        public void RevulyticsCollectUsageStatsForServerIsFalseTest()
        {
            Assert.AreEqual(false, AppUsageStats.CollectUsageStats);
        }

        [TestMethod]
        [Owner("Candice Daniel")]
        [TestCategory("RevulyticsCollectUsageStats")]
        public void RevulyticsCollectUsageStats_WhenNoConfigSetting_ShouldUseGlobalConstantValue()
        {
            GlobalConstants.CollectUsageStats = "True";
            Assert.AreEqual(true, AppUsageStats.CollectUsageStats);
            GlobalConstants.CollectUsageStats = null;
        }
    }
}
