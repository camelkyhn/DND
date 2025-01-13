using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DND.Middleware.Attributes;
using DND.Middleware.Dtos.Notification.Mail;
using DND.Middleware.System.Options;
using Microsoft.Extensions.Options;

namespace DND.Business.Services.Notification
{
    public interface IMailService
    {
        Task SendMailAsync(MailDto mailDto);
        Task SendCustomMailAsync(MailMessage mailMessage);
    }

    [ScopedDependency]
    public class MailService : Service, IMailService
    {
        private readonly SmtpOptions _smtpOptions;

        public MailService(IServiceProvider serviceProvider, IOptions<SmtpOptions> smtpOptions) : base(serviceProvider)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public async Task SendMailAsync(MailDto dto)
        {
            var smtpClient = GetSmtpClient();
            await smtpClient.SendMailAsync(new MailMessage(_smtpOptions.Username, string.Join(",", dto.ToEmailAddressList))
            {
                Body = dto.Message,
                Subject = dto.Subject,
                IsBodyHtml = !dto.IsBodyPlainText
            });
        }

        public async Task SendCustomMailAsync(MailMessage mailMessage)
        {
            var smtpClient = GetSmtpClient();
            await smtpClient.SendMailAsync(mailMessage);
        }

        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient
            {
                Host = _smtpOptions.Host,
                Port = _smtpOptions.Port,
                EnableSsl = false,
                Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password)
            };
        }
    }
}