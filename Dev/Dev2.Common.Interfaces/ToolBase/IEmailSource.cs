using Dev2.Common.Interfaces.Core.DynamicServices;
using System;

namespace Dev2.Common.Interfaces.ToolBase
{
    public interface IEmailSource : IEquatable<IEmailSource>
    {
        Guid ResourceID { get; set; }
        string ResourceName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        enSourceType Type { get; set; }
        string ResourceType { get; set; }
        string Path { get; set; }
        int Timeout { get; set; }
    }
}
