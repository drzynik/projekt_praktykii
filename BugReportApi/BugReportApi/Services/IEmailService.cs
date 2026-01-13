namespace BugReportApi.Services
{
    public interface IEmailService
    {
        Task SendAsync(string fromEmail, string description);

    }
}
