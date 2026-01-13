using System.Net;
using System.Net.Mail;

namespace BugReportApi.Services;

public class EmailService : IEmailService
{
    private const string SmtpHost = "smtp.gmail.com";
    private const int SmtpPort = 587;

    // PODAJ SWOJE DANE OUTLOOKA
    private const string SmtpUser = "drzynickibartosz@gmail.com";
    private const string SmtpPass = "wytm hcgb wbei ndsw";

    // Na ten adres będą przychodzić zgłoszenia
    private const string TargetEmail = "itm@itm.com.pl";

    public async Task SendAsync(string fromEmail, string description)
    {
        var message = new MailMessage
        {
            From = new MailAddress("no-reply@itm.com.pl", "Bug Reporter"),
            Subject = "Zgłoszenie błędu z aplikacji",
            Body = $"Email zgłaszającego: {fromEmail}\n\nOpis błędu:\n{description}",
            IsBodyHtml = false
        };

        message.To.Add(TargetEmail);

        using var client = new SmtpClient(SmtpHost, SmtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(SmtpUser, SmtpPass)
        };

        await client.SendMailAsync(message);

    }
}

