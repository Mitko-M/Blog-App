using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Junction table representing the many-to-many relationship between Posts and Tags.
    /// 
    /// Enables posts to have multiple tags and tags to be used across multiple posts.
    /// Tags provide fine-grained content labeling for better discovery and filtering.
    /// </summary>
    public class PostTag
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
        /// Foreign key to the Tag in this relationship.
        /// Part of the composite primary key.
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Navigation property to the Tag in this relationship.
        /// </summary>
        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; } = null!;
    }
}
