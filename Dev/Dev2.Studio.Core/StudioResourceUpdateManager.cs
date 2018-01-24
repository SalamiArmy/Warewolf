using System;
using System.Collections.Generic;
using System.Data;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.Deploy;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.ToolBase.Email;
using Dev2.Common.Interfaces.ToolBase.ExchangeEmail;
using Dev2.Common.Interfaces.WebServices;
using Dev2.ConnectionHelpers;
using Dev2.Controller;
using Dev2.Studio.Interfaces;

namespace Dev2.Studio.Core
{
    public class StudioResourceUpdateManager : IStudioUpdateManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="controllerFactory"/> is <see langword="null" />.</exception>
        public StudioResourceUpdateManager(ICommunicationControllerFactory controllerFactory, IEnvironmentConnection environmentConnection)
        {
            if (controllerFactory == null)
            {
                throw new ArgumentNullException(nameof(controllerFactory));
            }
            if (environmentConnection == null)
            {
                throw new ArgumentNullException(nameof(environmentConnection));
            }

            UpdateManagerProxy = new UpdateProxy(controllerFactory, environmentConnection);
        }

        public void FireServerSaved(Guid savedServerID) => FireServerSaved(savedServerID, false);
        public void FireServerSaved(Guid savedServerID, bool isDeleted)
        {
            if (ServerSaved != null)
            {
                var handler = ServerSaved;
                handler.Invoke(savedServerID, isDeleted);
            }
        }

        IUpdateManager UpdateManagerProxy { get; set; }

        public void Save(IServerSource source)
        {
            try
            {
                UpdateManagerProxy.SaveServerSource(source, GlobalConstants.ServerWorkspaceID);
                ConnectControlSingleton.Instance.ReloadServer();
                FireServerSaved(source.ID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save Server Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void Save(IPluginSource source)
        {
            try
            {
                UpdateManagerProxy.SavePluginSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save Plugin Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void Save(IComPluginSource source)
        {
            try
            {
                UpdateManagerProxy.SaveComPluginSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save Com Plugin Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void Save(IOAuthSource source)
        {
            try
            {
                UpdateManagerProxy.SaveOAuthSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save OAuth Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void Save(ISmtpSource source)
        {
            try
            {
                UpdateManagerProxy.SaveSmtpSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save Smtp Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void Save(IRabbitMQServiceSourceDefinition source)
        {
            try
            {
                UpdateManagerProxy.SaveRabbitMQServiceSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save RabbitMQ Source Error", ex, GlobalConstants.WarewolfError);
            }
        }
        public void Save(IExchangeSource source)
        {
            try
            {
                UpdateManagerProxy.SaveExchangeSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save Exchange Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void TestConnection(IServerSource serverSource)
        {
            UpdateManagerProxy.TestConnection(serverSource);
        }

        public string TestConnection(ISmtpSource smtpSource)
        {
            return UpdateManagerProxy.TestEmailServiceSource(smtpSource);
        }

        public string TestConnection(IRabbitMQServiceSourceDefinition rabbitMqServiceSource)
        {
            return UpdateManagerProxy.TestRabbitMQServiceSource(rabbitMqServiceSource);
        }
        public string TestConnection(IExchangeSource emailServiceSourceSource)
        {
            return UpdateManagerProxy.TestExchangeServiceSource(emailServiceSourceSource);
        }

        public void TestConnection(IWebServiceSource resource)
        {
            UpdateManagerProxy.TestConnection(resource);
        }

        public void TestConnection(ISharepointServerSource resource)
        {
            UpdateManagerProxy.TestConnection(resource);
        }

        public IList<string> TestDbConnection(IDbSource serverSource)
        {
            return UpdateManagerProxy.TestDbConnection(serverSource);
        }

        public void Save(IDbSource source)
        {
            try
            {
                UpdateManagerProxy.SaveDbSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save Database Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void Save(IWebService model)
        {
            UpdateManagerProxy.SaveWebservice(model, GlobalConstants.ServerWorkspaceID);
        }

        public void Save(IWebServiceSource source)
        {
            try
            {
                UpdateManagerProxy.SaveWebserviceSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save Web Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void Save(ISharepointServerSource source)
        {
            try
            {
                UpdateManagerProxy.SaveSharePointServiceSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save Sharepoint Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public void Save(IDatabaseService toDbSource)
        {
            UpdateManagerProxy.SaveDbService(toDbSource);
        }

        public DataTable TestDbService(IDatabaseService inputValues)
        {
            return UpdateManagerProxy.TestDbService(inputValues);
        }

        public string TestWebService(IWebService inputValues)
        {
            return UpdateManagerProxy.TestWebService(inputValues);
        }

        public string TestPluginService(IPluginService inputValues)
        {
            return UpdateManagerProxy.TestPluginService(inputValues);
        }

        public string TestPluginService(IComPluginService inputValues)
        {
            return UpdateManagerProxy.TestComPluginService(inputValues);
        }

        public void Save(IWcfServerSource source)
        {
            try
            {
                UpdateManagerProxy.SaveWcfSource(source, GlobalConstants.ServerWorkspaceID);
            }
            catch (Exception ex)
            {
                Dev2Logger.Error("Save WCF Source Error", ex, GlobalConstants.WarewolfError);
            }
        }

        public string TestWcfService(IWcfService inputValues)
        {
            return UpdateManagerProxy.TestWcfService(inputValues);
        }

        public string TestConnection(IWcfServerSource wcfServerSource)
        {
            return UpdateManagerProxy.TestWcfServiceSource(wcfServerSource);
        }

        public Action<Guid, bool> ServerSaved { get; set; }

        public List<IDeployResult> Deploy(List<Guid> resourceIDsToDeploy, bool deployTests, IConnection destinationEnvironment)
        {
            return UpdateManagerProxy.Deploy(resourceIDsToDeploy, deployTests, destinationEnvironment);
        }
    }
}