using System.Net.Mail;
using System.Net;
using System.Data;

namespace LomboReaderAPI.Services.Mail
{
    public class MailService : IMailService
    {
        private IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string mail, string code)
        {
            var gmailConfig = _configuration.GetSection("Gmail");
            String host = gmailConfig.GetValue<String>("Host");
            String box = gmailConfig.GetValue<String>("Box");
            String key = gmailConfig.GetValue<String>("Key");
            int port = gmailConfig.GetValue<int>("Port");
            bool ssl = gmailConfig.GetValue<bool>("Ssl");

            using SmtpClient smtpClient = new(host)
            {
                Port = port,
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)
            };

            try
            {

                MailMessage mailMessage = new MailMessage()
                {
                    IsBodyHtml = true,
                    From = new MailAddress(box),
                    Subject = "Подтверждение электронной почты",
                    Body = $"<h2 style='font-size:5em;color:red'>Limbo Reader Ваш код {code}</h2>",
                    
                };
                mailMessage.To.Add(new MailAddress(mail));

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
