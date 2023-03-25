namespace Catalog.API.Services.Messaging
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> emailAttachments = null!
        );
    }
}
