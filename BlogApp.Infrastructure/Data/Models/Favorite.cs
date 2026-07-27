using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a post marked as favorite by a user.
    /// 
    /// Users can bookmark/favorite posts for easy access later.
    /// This creates a personal collection of favorite posts for each user.
    /// </summary>
    public class Favorite
    {
        /// <summary>
        /// Primary key identifier for the favorite relationship.
        /// </summary>
        [Key]
        [Comment("Favorite identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the Post being favorited.
        /// </summary>
        [Required]
        [Comment("Post identifier")]
        public int PostId { get; set; }

        /// <summary>
        /// Navigation property to the Post being favorited.
        /// </summary>
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        /// <summary>
        /// Foreign key to the ApplicationUser who favorited this post.
        /// </summary>
        [Required]
        [Comment("Application user identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the ApplicationUser who created this favorite.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}
