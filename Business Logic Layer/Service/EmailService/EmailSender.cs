using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Business_Logic_Layer.Service.EmailService;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmail(string subject, string toEmail, string username, string Html, string message)
    {
        var apiKey = _configuration.GetValue<string>("SendGridApiKey")!;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("mennaosman7797@gmail.com", "Wedding Planner");
        var to = new EmailAddress(toEmail, username);
        var plainTextContent = message;
        var htmlContent = Html;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);
    }
}
