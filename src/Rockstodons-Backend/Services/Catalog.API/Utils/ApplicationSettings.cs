namespace Catalog.API.Utils
{
    public class ApplicationSettings
    {
        public string Secret { get; set; }

        public string SendGridAPIKey { get; set; }

        public string Email { get; set; }

        public string EmailSender { get; set; }

        public int MaxLoginFailedAttemptsCount { get; set; }

        public int FailedLoginWaitingTime { get; set; }

        public int MaxUnconfirmedEmailsCount { get; set; }

        public int UnconfirmedEmailWaitingTime { get; set; }

        public string ConfirmEmailUrl { get; set; }

        public int MaxResetPasswordCount { get; set; }

        public int ResetPasswordWaitingTime { get; set; }

        public int ResetPasswordValidityTime { get; set; }

        public string ResetPasswordUrl { get; set; }
    }
}
