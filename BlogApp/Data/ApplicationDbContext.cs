using BlogApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostCategory> PostsCategories { get; set; }
        public DbSet<PostTag> PostsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            //many-to-many configuration
            builder.Entity<PostCategory>(pc => pc.HasKey(e => new
            {
                e.PostId,
                e.CategoryId
            }));

            builder.Entity<PostCategory>()
                .HasOne(p => p.Post)
                .WithMany(c => c.PostsCategories)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PostCategory>()
                .HasOne(c => c.Category)
                .WithMany(p => p.PostsCategories)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PostTag>(pt => pt.HasKey(e => new
            {
                e.PostId,
                e.TagId
            }));

            builder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(c => c.PostsTags)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PostTag>()
                .HasOne(p => p.Tag)
                .WithMany(p => p.PostsCategories)
                .HasForeignKey(p => p.TagId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}
