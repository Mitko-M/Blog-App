using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Infrastructure.Data.Configuration
{
    internal class AdminConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            builder.HasData(ConfigurationHelper.Admin);
        }
    }
}
