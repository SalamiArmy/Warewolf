using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Help;
using Dev2.Studio.Interfaces;
using Moq;

namespace Dev2.Tests
{
    public static class ShellViewModelConstructor
    {
        public static Mock<IShellViewModel> ShellViewModelForTesting() => ShellViewModelForTesting(out Mock<IHelpWindowViewModel> mockHelpViewModel);
        public static Mock<IShellViewModel> ShellViewModelForTesting(out Mock<IHelpWindowViewModel> mockHelpViewModel)
        {
            var shellVm = new Mock<IShellViewModel>();
            var serverMock = new Mock<IServer>();
            var updateProxy = new Mock<IStudioUpdateManager>();
            var updateManager = new Mock<IQueryManager>();
            mockHelpViewModel = new Mock<IHelpWindowViewModel>();
            serverMock.Setup(server => server.UpdateRepository).Returns(updateProxy.Object);
            serverMock.Setup(server => server.QueryProxy).Returns(updateManager.Object);
            shellVm.Setup(model => model.ActiveServer).Returns(serverMock.Object);
            mockHelpViewModel.Setup(model => model.UpdateHelpText(It.IsAny<string>())).Verifiable();
            shellVm.Setup(model => model.HelpViewModel).Returns(mockHelpViewModel.Object);
            return shellVm;
        }
    }
}
