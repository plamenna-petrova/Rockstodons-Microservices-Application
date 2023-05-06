using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels.AccountViewModels
{
    public record ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
