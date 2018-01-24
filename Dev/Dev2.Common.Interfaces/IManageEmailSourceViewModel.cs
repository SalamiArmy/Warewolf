using Dev2.Common.Interfaces.ToolBase.Email;
using System;
using System.Windows.Input;

namespace Dev2.Common.Interfaces
{
    public interface IManageEmailSourceViewModel
    {
        string HostName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        int Port { get; set; }
        int Timeout { get; set; }
        string TestLabel { get; }
        string TestMessage { get; }
        bool TestFailed { get; set; }
        bool Testing { get; }
        bool TestPassed { get; set; }
        string EmailFrom { get; set; }
        string EmailTo { get; set; }
        string HostNameLabel { get; }
        string UserNameLabel { get; }
        string PasswordLabel { get; }
        string EnableSslLabel { get; }
        string PortLabel { get; }
        string TimeoutLabel { get; }
        string EmailFromLabel { get; }
        string EmailToLabel { get; }
        ICommand OkCommand { get; set; }
        ICommand SendCommand { get; set; }
        bool EnableSsl { get; set; }
        bool EnableSslYes { get; set; }
        bool EnableSslNo { get; set; }
    }

    public interface IManageEmailSourceModel
    {
        ISmtpSource FetchSource(Guid resourceID);
        string TestConnection(ISmtpSource resource);
        void Save(ISmtpSource toDbSource);
        string ServerName { get; }
    }
}
