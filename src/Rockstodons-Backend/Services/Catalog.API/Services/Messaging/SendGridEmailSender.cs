using SendGrid;
using SendGrid.Helpers.Mail;

namespace Catalog.API.Services.Messaging
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly SendGridClient _sendGridClient;

        public SendGridEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _sendGridClient = new SendGridClient(apiKey: _configuration["SendGrid:APIKey"]);
        }

        public async Task SendEmailAsync(
            string from, string fromName, string to, string subject,
            string htmlContent, IEnumerable<EmailAttachment> emailAttachments = null!
        )
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("The subject and message are required.");
            }

            var fromEmailAddress = new EmailAddress(from, fromName);
            var toEmailAddress = new EmailAddress(to);

            var message = MailHelper.CreateSingleEmail(
                fromEmailAddress, toEmailAddress, subject, null, htmlContent);

            if (emailAttachments?.Any() == true)
            {
                foreach (var emailAttachment in emailAttachments)
                {
                    message.AddAttachment(
                        emailAttachment.FileName,
                        Convert.ToBase64String(emailAttachment.Content),
                        emailAttachment.MimeType
                    );
                }
            }

            try
            {
                var response = await _sendGridClient.SendEmailAsync(message);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Body.ReadAsStringAsync());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
