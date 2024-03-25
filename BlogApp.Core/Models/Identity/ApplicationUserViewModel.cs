﻿namespace BlogApp.Core.Models.Identity
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int PostCount { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
