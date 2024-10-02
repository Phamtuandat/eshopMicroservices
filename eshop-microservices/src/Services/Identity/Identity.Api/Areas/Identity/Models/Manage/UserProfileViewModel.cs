using Identity.Api.Enums;

namespace Identity.Api.Areas.Identity.Models.Manage
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Birthday { get; set; } = default!;
        public Gender Gender { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string AddressLine { get; set; } = default!;
        public string ProfilePicture { get; set; } = default!;

    }
}
