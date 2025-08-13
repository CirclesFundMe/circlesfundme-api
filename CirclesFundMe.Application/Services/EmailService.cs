namespace CirclesFundMe.Application.Services
{
    public class EmailService(IOptions<AppSettings> appSettings, ILogger<EmailService> logger, IHostEnvironment hostEnvironment)
    {
        private readonly MailSettings _mailSettings = appSettings.Value.MailSettings;
        private readonly ILogger<EmailService> _logger = logger;
        private readonly IHostEnvironment _hostEnvironment = hostEnvironment;

        public async Task<bool> SendEmail(EmailMessage message)
        {
            bool result = false;

            var emailMsg = CreateEmailMessage(message);

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_mailSettings.SmtpServer, 26, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(_mailSettings.EmailUsername, _mailSettings.EmailPassword);

                await client.SendAsync(emailMsg);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email - {ex.Message}\n");
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            return result;
        }
        public async Task<List<bool>> SendEmailBatchAsync(List<EmailMessage> messages)
        {
            var results = new List<bool>();
            int batchSize = 100;

            for (int i = 0; i < messages.Count; i += batchSize)
            {
                var batch = messages.Skip(i).Take(batchSize).ToList();

                using var client = new SmtpClient();
                try
                {
                    await client.ConnectAsync(_mailSettings.SmtpServer, 26, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_mailSettings.EmailUsername, _mailSettings.EmailPassword);

                    foreach (var message in batch)
                    {
                        try
                        {
                            var emailMsg = CreateEmailMessage(message);
                            await client.SendAsync(emailMsg);
                            results.Add(true);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error sending email to {message.To.Address} - {ex.Message}\n");
                            results.Add(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Batch SMTP error - {ex.Message}\n");
                    // Mark all in batch as failed
                    results.AddRange(Enumerable.Repeat(false, batch.Count));
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
            return results;
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            MimeMessage emailMsg = new();
            emailMsg.From.Add(new MailboxAddress("CirclesFundMe", _mailSettings.SmtpFrom));
            emailMsg.To.Add(message.To);
            emailMsg.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder()
            {
                HtmlBody = message.Content
            };

            if (message.HasAttachment && message.Attachements != null && message.Attachements.Count != 0)
            {
                foreach (var attachment in message.Attachements)
                {
                    bodyBuilder.Attachments.Add(attachment.FileName, Convert.FromBase64String(attachment.FileBase64String), ContentType.Parse(attachment.ContentType));
                }
            }
            emailMsg.Body = bodyBuilder.ToMessageBody();
            return emailMsg;
        }

        public string LoadHtmlTemplate(string templateFilename)
        {
            string templatePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "templates", templateFilename) + ".html";
            string templateContent = File.ReadAllText(templatePath);
            return templateContent;
        }
    }
    public class EmailMessage(string to, string subject, string content, List<Attachement>? attachements, bool hasAttachment = false)
    {
        public MailboxAddress To { get; set; } = new MailboxAddress("", to);
        public string Subject { get; set; } = subject;
        public string Content { get; set; } = content;
        public List<Attachement> Attachements { get; set; } = attachements ?? [];
        public bool HasAttachment { get; set; } = hasAttachment;
    }
    public record Attachement
    {
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public required string FileBase64String { get; set; }
    }
}
