using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Junction table representing the many-to-many relationship between Posts and Categories.
    /// 
    /// Enables posts to belong to multiple categories and categories to contain multiple posts.
    /// This provides flexible content organization and discovery.
    /// </summary>
    public class PostCategory
    {
        /// <summary>
        /// Foreign key to the Post in this relationship.
        /// Part of the composite primary key.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Navigation property to the Post in this relationship.
        /// </summary>
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        /// <summary>
        /// Foreign key to the Category in this relationship.
        /// Part of the composite primary key.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Navigation property to the Category in this relationship.
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
    }
}
