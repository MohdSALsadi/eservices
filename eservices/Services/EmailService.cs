using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pattern_of_life
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        private readonly ILogger<EmailService> logger;
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            this.emailSettings = emailSettings.Value;
            this.logger = logger;
        }
        public async Task SendEmailAsync(EmailRequest request)
        {
            MimeMessage email = new()
            {
                Sender = MailboxAddress.Parse(emailSettings.Sender)
            };
            email.To.Add(MailboxAddress.Parse(request.Receiver));
            email.Subject = request.Subject;
            BodyBuilder builder = new();
            if (request.Attachments != null)
            {
                byte[] fileBytes;
                foreach (Microsoft.AspNetCore.Http.IFormFile file in request.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (MemoryStream ms = new())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                          builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();

            using SmtpClient client = new();
            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                //client.Connect(emailSettings.MailServer, emailSettings.MailPort, true);
                client.Connect(emailSettings.MailServer, emailSettings.MailPort);
                //client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(emailSettings.Sender, emailSettings.Password);
                  await client.SendAsync(email);

                logger.LogInformation($"Email send successfully to {request.Receiver}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unhandled exception occured in {nameof(EmailService)} - {nameof(this.SendEmailAsync)}");
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
