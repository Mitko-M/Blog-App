using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Infrastructure.Data.Configuration
{
    public class PostCategoriesConfiguration : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            builder.HasData(new PostCategory[]
            {
                new PostCategory()
                {
                    PostId = 1,
                    CategoryId = 1
                },
                new PostCategory()
                {
                    PostId = 2,
                    CategoryId = 2
                },
                new PostCategory()
                {
                    PostId = 3,
                    CategoryId = 3
                }
            });
        }
    }
}
