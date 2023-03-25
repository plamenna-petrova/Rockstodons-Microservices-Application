namespace Catalog.API.Services.Messaging
{
    public class NullMessageSender : IEmailSender
    {
        public Task SendEmailAsync(
            string from, 
            string fromName, 
            string to, 
            string subject, 
            string htmlContent, 
            IEnumerable<EmailAttachment> emailAttachments = null!)
        {
            return Task.CompletedTask;
        }
    }
}
