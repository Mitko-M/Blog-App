using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace BlogApp.Infrastructure.Data.Configuration
{
    public static class ConfigurationHelper
    {
        private static PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

        public static IdentityUser Admin = GetAdmin();

        //categories

        public static Category[] SeedCategories()
        {
            string dir = GetDirectory();

            string path = dir + @"../BlogApp.Infrastructure/Data/Configuration/categoryconfig.json";

            string jsonString = File.ReadAllText(path);

            var categories = JsonConvert.DeserializeObject<Category[]>(jsonString);

            return categories;
        }

        //tags

        public static Tag[] SeedingTags()
        {
            string dir = GetDirectory();

            string path = dir + @"../BlogApp.Infrastructure/Data/Configuration/tagconfig.json";

            string jsonString = File.ReadAllText(path);

            var tags = JsonConvert.DeserializeObject<Tag[]>(jsonString);

            return tags;
        }

        private static ApplicationUser GetAdmin()
        {
            var adminUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
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

        private static string GetDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryName = Path.GetFileName(currentDirectory);
            var relativePath = directoryName.StartsWith("net6.0") ? @"../../../" : string.Empty;

            return relativePath;
        }
    }
}
