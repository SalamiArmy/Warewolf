using System.Collections.ObjectModel;

namespace Dev2.Common.Interfaces.ToolBase.Email
{
    public interface ISmtpServiceModel
    {
        ObservableCollection<ISmtpSource> RetrieveSources();
        void CreateNewSource();
        void EditSource(ISmtpSource selectedSource);
        IStudioUpdateManager UpdateRepository { get; }
    }
}
