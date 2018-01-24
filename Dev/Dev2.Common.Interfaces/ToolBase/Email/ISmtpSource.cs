using System;

namespace Dev2.Common.Interfaces.ToolBase.Email
{
    public interface ISmtpSource : IEmailSource, IEquatable<ISmtpSource>
    {
        string Host { get; set; }
        string EmailFrom { get; set; }
        string EmailTo { get; set; }
        int Port { get; set; }
        bool EnableSSL { get; set; }
    }
}
