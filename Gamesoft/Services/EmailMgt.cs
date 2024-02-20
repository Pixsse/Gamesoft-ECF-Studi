
using Gamesoft.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Gamesoft.Services
{
    public class EmailMgt : IEmailMgt
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailMgt(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public bool SendEmail(string to, string subject, string body)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.Username),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(to);

                    client.Send(mailMessage);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
