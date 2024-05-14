using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Pages.Create
{
    public class InputModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        [Display(Name = "Password confirmation")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public bool RememberLogin { get; set; }
        public string? ReturnUrl { get; set; }
        public string? Button { get; set; }
    }
}
