using Identity.Api.Enums;

namespace Identity.Api.Areas.Identity.Models.Manage
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string Email { get; set; }
        public string Birthday { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string ProfilePicture { get; set; } 

    }
}
