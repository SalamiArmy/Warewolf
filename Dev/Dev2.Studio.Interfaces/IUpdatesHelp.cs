using Dev2.Studio.Interfaces;

namespace Dev2.Common.Interfaces
{
    public interface IUpdatesHelp
    {
        void UpdateHelpDescriptor(string helpText, IShellViewModel mainViewModel);
    }
}