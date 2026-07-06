using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a "like" vote on a comment.
    /// 
    /// Users can like comments from other users to indicate appreciation or agreement.
    /// This establishes a many-to-many relationship through a junction table.
    /// </summary>
    public class CommentLike
    {
        /// <summary>
        /// Primary key identifier for the comment like relationship.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the Comment being liked.
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// Navigation property to the Comment being liked.
        /// </summary>
        [ForeignKey(nameof(CommentId))]
        public Comment Comment { get; set; } = null!;

        /// <summary>
        /// Foreign key to the ApplicationUser who liked this comment.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the ApplicationUser who created this like.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}
