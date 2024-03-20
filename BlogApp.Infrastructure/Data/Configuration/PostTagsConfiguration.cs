using BlogApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Infrastructure.Data.Configuration
{
    public class PostTagsConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.HasData(new PostTag[]
            {
                new PostTag()
                {
                    PostId = 1,
                    TagId = 1,
                },
                new PostTag()
                {
                    PostId = 2,
                    TagId = 2,
                },
                new PostTag()
                {
                    PostId = 3,
                    TagId = 3,
                }
            });
        }
    }
}
