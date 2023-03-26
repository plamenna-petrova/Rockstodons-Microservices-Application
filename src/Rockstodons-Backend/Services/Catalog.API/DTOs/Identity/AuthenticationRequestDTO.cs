using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Identity
{
    public class AuthenticationRequestDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
