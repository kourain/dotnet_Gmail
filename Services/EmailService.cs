using GmailService.Classes;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace GmailService.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailRequest request);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(EmailRequest request)
        {
            // try
            {
                var email = new MimeMessage();

                // From
                email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));

                // To
                foreach (var recipient in request.To)
                {
                    email.To.Add(MailboxAddress.Parse(recipient));
                }

                // CC
                if (request.Cc != null)
                {
                    foreach (var cc in request.Cc)
                    {
                        email.Cc.Add(MailboxAddress.Parse(cc));
                    }
                }

                // BCC
                if (request.Bcc != null)
                {
                    foreach (var bcc in request.Bcc)
                    {
                        email.Bcc.Add(MailboxAddress.Parse(bcc));
                    }
                }

                // Subject
                email.Subject = request.Subject;

                // Body
                var bodyFormat = request.IsHtml ? TextFormat.Html : TextFormat.Plain;
                var body = new TextPart(bodyFormat)
                {
                    Text = request.Body
                };

                // Create message body
                var multipart = new Multipart("mixed");
                multipart.Add(body);

                // Attachments
                if (request.Attachments != null && request.Attachments.Any())
                {
                    foreach (var attachment in request.Attachments)
                    {
                        var attachmentPart = new MimePart(attachment.ContentType)
                        {
                            Content = new MimeContent(new MemoryStream(attachment.Content)),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = attachment.FileName
                        };

                        multipart.Add(attachmentPart);
                    }
                }

                email.Body = multipart;

                // Connect to SMTP server
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);

                // Authenticate
                await smtp.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

                // Send email
                await smtp.SendAsync(email);

                // Disconnect
                await smtp.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to {string.Join(", ", request.To)}");
                return true;
            }
            // catch (Exception ex)
            // {
            //     _logger.LogError($"Failed to send email: {ex.Message}");
            //     return false;
            // }
        }
    }
}