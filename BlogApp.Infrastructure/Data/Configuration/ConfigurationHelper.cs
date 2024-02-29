using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;

namespace BlogApp.Infrastructure.Data.Configuration
{
    public static class ConfigurationHelper
    {
        private static PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();

        public static IdentityUser Admin = GetAdmin();

        public static IdentityUser TestUser = GetUser();

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

        private static IdentityUser GetUser()
        {
            var user = new IdentityUser()
            {
                Email = "user@blog.com",
                NormalizedEmail = "USER@BLOG.COM"
            };

            user.PasswordHash = hasher.HashPassword(user, "userpass");

            return user;
        }

        private static IdentityUser GetAdmin()
        {
            var adminUser = new IdentityUser()
            {
                Email = "admin@blog.com",
                NormalizedEmail = "ADMIN@BLOG.COM"
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "adminpass");

            return adminUser;
        }
    }
}
