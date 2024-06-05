using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Areas.Identity.Models.Manage
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string OtpCode { get; set; }

        public string? ReturnUrl { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
