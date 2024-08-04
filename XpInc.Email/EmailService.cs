using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using XpInc.Email.Configuration;

namespace XpInc.Email
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> _settings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options;
        }
        public async Task EnviaEmail(string toEmail, string toName, string fromEmail, string fromName, string subject, string plainTextContent, string htmlContent)
        {
            string apiKey = _settings.Value.SendGridToken;

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail, toName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            response.StatusCode.ToString();
        }
    }
}
