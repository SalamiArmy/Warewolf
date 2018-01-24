using System;

namespace Dev2.Common.Interfaces.ToolBase.ExchangeEmail
{
    public interface IExchangeSource : IEmailSource, IEquatable<IExchangeSource>
    {
        string AutoDiscoverUrl { get; set; }
    }
}