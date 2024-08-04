using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace XpInc.Email
{
    public class EmailService : IEmailService
    {
        private readonly string SendGridToken = "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";
        public EmailService()
        {
        }
        public async Task EnviaEmail(string toEmail, string toName, string fromEmail, string fromName, string subject, string plainTextContent, string htmlContent)
        {
            string apiKey = SendGridToken;

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail, toName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            response.StatusCode.ToString();
        }
    }
}
