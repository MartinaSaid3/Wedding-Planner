using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.EmailService
{
    public class EmailSender:IEmailSender
    {
        public async Task SendEmail(string subject, string toEmail, string username, string message, string Html)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("mennaosman7797@gmail.com", "Wedding Planner");
            var to = new EmailAddress(toEmail, username);
            var plainTextContent = message;
            var htmlContent = Html;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
