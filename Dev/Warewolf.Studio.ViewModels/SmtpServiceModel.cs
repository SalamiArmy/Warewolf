using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.ToolBase.Email;
using Dev2.Studio.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warewolf.Studio.ViewModels
{
    public class SmtpServiceModel : ISmtpServiceModel
    {
        readonly IStudioUpdateManager _updateRepository;
        readonly IQueryManager _queryProxy;
        readonly IShellViewModel _shell;

        public SmtpServiceModel(IStudioUpdateManager updateRepository, IQueryManager queryProxy, IShellViewModel shell, IServer server)
        {
            _updateRepository = updateRepository;
            _queryProxy = queryProxy;
            _shell = shell;
            shell.SetActiveServer(server.EnvironmentID);
        }
        public ObservableCollection<ISmtpSource> RetrieveSources()
        {
            return new ObservableCollection<ISmtpSource>(_queryProxy.FetchSmtpSources());
        }

        public void CreateNewSource()
        {
            _shell.NewEmailSource(string.Empty);
        }

        public void EditSource(ISmtpSource selectedSource)
        {
            _shell.EditResource(selectedSource);
        }


        public IStudioUpdateManager UpdateRepository => _updateRepository;
    }
}
