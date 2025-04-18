﻿using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
