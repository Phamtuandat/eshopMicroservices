using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Areas.Identity.Models.Account.Register
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;


        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = default!;


        [DataType(DataType.Text)]
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; } = default!;

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = default!;

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = default!;

        public string? ReturnUrl { get; set; } = default!;


        public bool RememberLogin { get; set; } = default!;
        public string? Button { get; set; } = "register";


    }
}
