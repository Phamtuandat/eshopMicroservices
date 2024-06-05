// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Identity.Api.Enums;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AddressLine { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}
