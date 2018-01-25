using Dev2.Common.ExtMethods;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Enums;
using Dev2.Communication;
using Dev2.Runtime.ESB.Management.Services;
using Dev2.Runtime.Interfaces;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dev2.Tests.Runtime.Services
{
    [TestClass]
    public class SaveEmailServiceSourceTests
    {
        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("GetResourceID")]
        public void GetResourceID_ShouldReturnEmptyGuid()
        {
            //------------Setup for test--------------------------
            var saveEmailServiceSource = new SaveEmailServiceSource();
            //------------Execute Test---------------------------
            var resId = saveEmailServiceSource.GetResourceID(new Dictionary<string, StringBuilder>());
            //------------Assert Results-------------------------
            Assert.AreEqual(Guid.Empty, resId);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("GetResourceID")]
        public void GetAuthorizationContextForService_ShouldReturnContext()
        {
            //------------Setup for test--------------------------
            var saveEmailServiceSource = new SaveEmailServiceSource();
            //------------Execute Test---------------------------
            var resId = saveEmailServiceSource.GetAuthorizationContextForService();
            //------------Assert Results-------------------------
            Assert.AreEqual(AuthorizationContext.Contribute, resId);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SaveEmailServiceSource_HandlesType")]
        public void SaveEmailServiceSource_HandlesType_ExpectName()
        {
            //------------Setup for test--------------------------
            var saveEmailServiceSource = new SaveEmailServiceSource();
            //------------Execute Test---------------------------
            //------------Assert Results-------------------------
            Assert.AreEqual("SaveEmailServiceSource", saveEmailServiceSource.HandlesType());
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SaveEmailServiceSource_HandlesType")]
        public void SaveEmailServiceSource_CreateServiceEntry_ExpectActions()
        {
            //------------Setup for test--------------------------
            var saveEmailServiceSource = new SaveEmailServiceSource();
            //------------Execute Test---------------------------
            var dynamicService = saveEmailServiceSource.CreateServiceEntry();
            //------------Assert Results-------------------------
            Assert.IsNotNull(dynamicService);
            Assert.IsNotNull(dynamicService.Actions);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SaveEmailServiceSource_Execute")]
        public void SaveEmailServiceSource_Execute_NullValues_ErrorResult()
        {
            //------------Setup for test--------------------------
            var saveEmailServiceSource = new SaveEmailServiceSource();
            var serializer = new Dev2JsonSerializer();
            //------------Execute Test---------------------------
            var jsonResult = saveEmailServiceSource.Execute(null, null);
            var result = serializer.Deserialize<ExecuteMessage>(jsonResult);
            //------------Assert Results-------------------------
            Assert.IsTrue(result.HasError);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SaveEmailServiceSource_Execute")]
        public void SaveEmailServiceSource_Execute_ResourceIDNotPresent_ErrorResult()
        {
            //------------Setup for test--------------------------
            var values = new Dictionary<string, StringBuilder> { { "item", new StringBuilder() } };
            var saveEmailServiceSource = new SaveEmailServiceSource();
            var serializer = new Dev2JsonSerializer();
            //------------Execute Test---------------------------
            var jsonResult = saveEmailServiceSource.Execute(values, null);
            var result = serializer.Deserialize<ExecuteMessage>(jsonResult);
            //------------Assert Results-------------------------
            Assert.IsTrue(result.HasError);
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SaveEmailServiceSource_Execute_GivenResourceDefination")]
        public void Execute_GivenResourceDefination_ShouldSaveNewSourceReturnResourceDefinationMsg()
        {
            //---------------Set up test pack-------------------
            var serializer = new Dev2JsonSerializer();
            var source = new EmailServiceSourceDefinition
            {
                Host = "TestHost",
                UserName = "",
                Password = "",
                Port = 25,
                EnableSSL = true,
                Timeout = 10000,
                ResourceName = "Name",
                ResourceID = Guid.Empty,
                Type = enSourceType.EmailSource,
                ResourceType = "EmailSource"
            };
            var compressedExecuteMessage = new CompressedExecuteMessage();
            var serializeToJsonString = source.SerializeToJsonString(new DefaultSerializationBinder());
            compressedExecuteMessage.SetMessage(serializeToJsonString);
            var values = new Dictionary<string, StringBuilder>
            {
                { "EmailServiceSource", source.SerializeToJsonStringBuilder() }
            };
            var catalog = new Mock<IResourceCatalog>();
            catalog.Setup(resourceCatalog => resourceCatalog.SaveResource(It.IsAny<Guid>(), It.IsAny<IResource>(), It.IsAny<string>()));
            var saveEmailSource = new SaveEmailServiceSource
            {
                ResourceCatalogue = catalog.Object
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var jsonResult = saveEmailSource.Execute(values, null);
            var result = serializer.Deserialize<ExecuteMessage>(jsonResult);
            //---------------Test Result -----------------------
            Assert.IsFalse(result.HasError);
            catalog.Verify(resourceCatalog => resourceCatalog.SaveResource(It.IsAny<Guid>(), It.IsAny<IResource>(), It.IsAny<string>()));
        }

        [TestMethod]
        [Owner("Pieter Terblanche")]
        [TestCategory("SaveEmailServiceSource_Execute_GivenExistingResourceDefination")]
        public void Execute_GivenResourceDefination_GivenExising_ShouldReturnResourceDefinationMsg()
        {
            //---------------Set up test pack-------------------
            var serializer = new Dev2JsonSerializer();
            var source = new EmailServiceSourceDefinition
            {
                Host = "TestHost",
                UserName = "",
                Password = "",
                Port = 25,
                EnableSSL = true,
                Timeout = 10000,
                ResourceName = "Name",
                ResourceID = Guid.Empty,
                Type = enSourceType.EmailSource,
                ResourceType = "EmailSource"
            };
            var compressedExecuteMessage = new CompressedExecuteMessage();
            var serializeToJsonString = source.SerializeToJsonString(new DefaultSerializationBinder());
            compressedExecuteMessage.SetMessage(serializeToJsonString);
            var values = new Dictionary<string, StringBuilder>
            {
                { "EmailServiceSource", source.SerializeToJsonStringBuilder() }
            };
            var catalog = new Mock<IResourceCatalog>();
            var emailSource = new EmailSource();
            catalog.Setup(resourceCatalog => resourceCatalog.SaveResource(It.IsAny<Guid>(), emailSource, It.IsAny<string>()));
            var saveEmailSource = new SaveEmailServiceSource
            {
                ResourceCatalogue = catalog.Object
            };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var jsonResult = saveEmailSource.Execute(values, null);
            var result = serializer.Deserialize<ExecuteMessage>(jsonResult);
            //---------------Test Result -----------------------
            Assert.IsFalse(result.HasError);
            catalog.Verify(resourceCatalog => resourceCatalog.SaveResource(It.IsAny<Guid>(), emailSource, It.IsAny<string>()));
        }
    }
}
