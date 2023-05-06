using System.ComponentModel.DataAnnotations;

namespace Identity.API.ViewModels.AccountViewModels
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; init; }

        [Required]
        public string Codde { get; init; }

        public string ReturnUrl { get; init; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; init; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; init; }
    }
}
