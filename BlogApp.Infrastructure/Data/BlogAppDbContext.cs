using BlogApp.Infrastructure.Data.Configuration;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Data
{
    public class BlogAppDbContext : IdentityDbContext
    {
        public BlogAppDbContext(DbContextOptions<BlogAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> AppUser {  get; set; }    

        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<PostCategory> PostsCategories { get; set; }

        public DbSet<PostTag> PostsTags { get; set; }

        public DbSet<LikeDislike> LikesDislikes { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //comments model
            builder.Entity<Comment>()
                .HasOne(f => f.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            //likes dislikes model
            builder.Entity<LikeDislike>()
                .HasOne(f => f.Post)
                .WithMany(p => p.LikesDislikes)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            //favorites model
            builder.Entity<Favorite>()
                .HasOne(f => f.Post)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PostId)
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
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostCategory>()
                .HasOne(c => c.Category)
                .WithMany(p => p.PostsCategories)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostTag>(pt => pt.HasKey(e => new
            {
                e.PostId,
                e.TagId
            }));

            builder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(c => c.PostsTags)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PostTag>()
                .HasOne(p => p.Tag)
                .WithMany(p => p.PostsTags)
                .HasForeignKey(p => p.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            //seeding
            builder.ApplyConfiguration(new RolesConfiguration());
            builder.ApplyConfiguration(new AdminConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new PostConfiguration());
            builder.ApplyConfiguration(new PostCategoriesConfiguration());
            builder.ApplyConfiguration(new PostTagsConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
