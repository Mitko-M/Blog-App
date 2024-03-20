using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Infrastructure.Data.Configuration
{
    public static class ConfigurationHelper
    {
        private static PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

        public static IdentityUser Admin = GetAdmin();

        //categories

        public static Category Science = new Category()
        {
            Id = 1,
            Name = "Science",
        };

        public static Category Nature = new Category()
        {
            Id = 2,
            Name = "Nature",
        };

        public static Category IT = new Category()
        {
            Id = 3,
            Name = "IT and Computer Science",
        };

        //tags

        public static Tag Funny = new Tag()
        {
            Id = 1,
            Name = "Funny"
        };

        public static Tag Interesting = new Tag()
        {
            Id = 2,
            Name = "Interesting"
        };

        public static Tag Boring = new Tag()
        {
            Id= 3,
            Name = "Boring"
        };

        private static ApplicationUser GetAdmin()
        {
            var adminUser = new ApplicationUser()
            {
                Id = "1",
                FirstName = "Mitko",
                LastName = "Mitkov",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@blog.com",
                NormalizedEmail = "ADMIN@BLOG.COM"
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "mitkov");

            return adminUser;
        }
    }
}
