using Kentimce.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpHost = _configuration["EmailSettings:Host"];
            var smtpPort = int.Parse(_configuration["EmailSettings:Port"]);
            var smtpUser = _configuration["EmailSettings:User"];
            var smtpPass = _configuration["EmailSettings:Password"];
            
            var mail = new MailMessage();
            mail.From = new MailAddress(smtpUser, "Kentimce");
            mail.To.Add(new MailAddress(to));
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}

