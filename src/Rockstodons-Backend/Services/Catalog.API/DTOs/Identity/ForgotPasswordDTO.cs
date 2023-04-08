using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Identity
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
