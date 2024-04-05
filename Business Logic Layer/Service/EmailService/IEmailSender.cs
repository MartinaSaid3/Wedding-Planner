using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service.EmailService
{
    public interface IEmailSender
    {
       Task SendEmail(string subject, string toEmail, string username, string message, string Html);
    }
}
