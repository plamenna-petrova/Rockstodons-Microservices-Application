﻿using System.ComponentModel.DataAnnotations;

namespace Catalog.API.DTOs.Identity
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "The username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "The confirmation password is required.")]
        [Compare("Password", ErrorMessage = "The password and the confirmation password must match.")]
        public string ConfirmPassword { get; set; }
    }
}
