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
        public async Task SendEmail(string subject, string toEmail, string username, string Html, string message)
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
