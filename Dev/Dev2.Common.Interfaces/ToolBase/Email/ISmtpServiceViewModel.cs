namespace Dev2.Common.Interfaces.ToolBase.Email
{
    public interface ISmtpServiceViewModel
    {
        ISourceToolRegion<ISmtpSource> SourceRegion { get; set; }
    }
}
