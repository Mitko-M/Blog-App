using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Data.Configuration
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasData(new Post[]
            {
                new Post()
                {
                    Id = 1,
                    Title = "My First Post",
                    Content = "This is my first post's content",
                    ShortDescription = "This is my post's short description",
                    CreatedOn = DateTime.Now.AddMonths(-60),
                    UpdatedOn = DateTime.Now.AddMonths(-30),
                    UserId = ConfigurationHelper.TestUser.Id,
                },
                new Post()
                {
                    Id = 2,
                    Title = "My Second Post",
                    Content = "This is my second post's content",
                    ShortDescription = "This is my post's short description",
                    CreatedOn = DateTime.Now.AddYears(-5),
                    UpdatedOn = DateTime.Now.AddMonths(-10),
                    UserId = ConfigurationHelper.TestUser.Id,
                },
                new Post()
                {
                    Id = 3,
                    Title = "My Third Post",
                    Content = "This is my third post's content",
                    ShortDescription = "This is my post's short description",
                    CreatedOn = DateTime.Now.AddDays(-60),
                    UpdatedOn = DateTime.Now.AddDays(-5),
                    UserId = ConfigurationHelper.TestUser.Id,
                }
            });
        }
    }
}
