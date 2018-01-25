using System;
using Dev2.Common.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.ToolBase.Email;
using Dev2.Runtime.ServiceModel.Data;

namespace Warewolf.Studio.ViewModels
{
    public class ManageEmailSourceModel : IManageEmailSourceModel
    {
        readonly IStudioUpdateManager _updateRepository;
        readonly IQueryManager _queryProxy;

        public ManageEmailSourceModel(IStudioUpdateManager updateRepository, IQueryManager queryProxy, string serverName)
        {
            _updateRepository = updateRepository;
            _queryProxy = queryProxy;

            ServerName = serverName;
            if (ServerName.Contains("("))
            {
                ServerName = serverName.Substring(0, serverName.IndexOf("(", StringComparison.Ordinal));
            }
        }

        public ISmtpSource FetchSource(Guid resourceID)
        {
            var xaml = _queryProxy.FetchResourceXaml(resourceID);
            var db = new EmailSource(xaml.ToXElement());

            var def = new EmailServiceSourceDefinition
            {
                Id = db.ResourceID,
                ResourceID = db.ResourceID,
                Host = db.Host,
                Password = db.Password,
                UserName = db.UserName,
                Path = "",
                Port = db.Port,
                Timeout = db.Timeout,
                ResourceName = db.ResourceName,
                EnableSSL = db.EnableSsl
            };
            return def;
        }

        public string TestConnection(ISmtpSource resource)
        {
            return _updateRepository.TestConnection(resource);
        }

        public void Save(ISmtpSource toDbSource)
        {
            _updateRepository.Save(toDbSource);
        }

        public string ServerName { get; private set; }
    }
}