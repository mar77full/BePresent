using BePresent.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BePresent.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger; 
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger= logger;
        }
       
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                _logger.LogInformation("sending email");
                _logger.LogError("sending email error loged");
                var emailSettings = _configuration.GetSection("EmailSettings");
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Sender", emailSettings["SenderEmail"]));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };

                emailMessage.Body = bodyBuilder.ToMessageBody();
                using (var smtpClient = new SmtpClient())
                {

                    await smtpClient.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
                    await smtpClient.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);

                    await smtpClient.SendAsync(emailMessage);

                    await smtpClient.DisconnectAsync(true);
                }
            }
            catch (Exception exc) {

                _logger.LogError(exc.Message);
            }
        }
    }
}
