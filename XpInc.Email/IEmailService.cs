using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpInc.Email
{
    public interface IEmailService
    {
        Task EnviaEmail(string toEmail, string toName, string fromEmail, string fromName, string subject, string plainTextContent, string htmlContent);
    }
}
