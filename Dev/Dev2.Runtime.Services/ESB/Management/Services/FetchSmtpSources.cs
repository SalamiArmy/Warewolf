using Dev2.Common;
using Dev2.Common.Interfaces.Core;
using Dev2.Communication;
using Dev2.DynamicServices;
using Dev2.Runtime.Hosting;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Workspaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev2.Runtime.ESB.Management.Services
{
    public class FetchSmtpSources : DefaultEsbManagementEndpoint
    {
        public override StringBuilder Execute(Dictionary<string, StringBuilder> values, IWorkspace theWorkspace)
        {
            var serializer = new Dev2JsonSerializer();

            var list = Resources.GetResourceList(GlobalConstants.ServerWorkspaceID).Where(a => a.ResourceType == "EmailSource").Select(a =>
            {
                var res = Resources.GetResource<EmailSource>(GlobalConstants.ServerWorkspaceID, a.ResourceID);
                if (res != null)
                {
                    return new EmailServiceSourceDefinition
                    {
                        Id = res.ResourceID,
                        Host = res.Host,
                        Password = res.Password,
                        UserName = res.UserName,
                        Path = res.GetSavePath(),
                        Port = res.Port,
                        Timeout = res.Timeout,
                        ResourceName = res.ResourceName,
                        EnableSSL = res.EnableSsl
                    };
                }
                return null;
            }).ToList();
            return serializer.SerializeToBuilder(new ExecuteMessage { HasError = false, Message = serializer.SerializeToBuilder(list) });
        }

        ResourceCatalog Resources => ResourceCatalog.Instance;

        public override DynamicService CreateServiceEntry() => EsbManagementServiceEntry.CreateESBManagementServiceEntry(HandlesType(), "<DataList><Dev2System.ManagmentServicePayload ColumnIODirection=\"Both\"></Dev2System.ManagmentServicePayload></DataList>");

        public override string HandlesType() => "FetchSmtpSources";
    }
}
