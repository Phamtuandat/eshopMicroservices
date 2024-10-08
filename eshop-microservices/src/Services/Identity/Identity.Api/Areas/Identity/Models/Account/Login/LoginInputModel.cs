﻿using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Areas.Identity.Models.Account.Login
{
    public class LoginInputModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool RememberLogin { get; set; }
        public string? Button { get; set; } = "login";
    }
}
