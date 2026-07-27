using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a like or dislike vote on a blog post.
    /// 
    /// Users can like or dislike posts. This model tracks both positive (liked=true)
    /// and negative (liked=false) votes. The combination of likes/dislikes helps
    /// surface popular or controversial content.
    /// </summary>
    public class LikeDislike
    {
        /// <summary>
        /// Primary key identifier for the like/dislike vote.
        /// </summary>
        [Key]
        [Comment("LikeDislike identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Boolean flag indicating the vote type.
        /// True = Like (positive vote)
        /// False = Dislike (negative vote)
        /// </summary>
        [Required]
        [Comment("Boolean which determines whether something is liked or not")]
        public bool Liked { get; set; }

        /// <summary>
        /// Foreign key to the Post being voted on.
        /// </summary>
        [Required]
        [Comment("Post identifier")]
        public int PostId { get; set; }

        /// <summary>
        /// Navigation property to the Post being voted on.
        /// </summary>
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        /// <summary>
        /// Foreign key to the ApplicationUser who cast this vote.
        /// </summary>
        [Required]
        [Comment("Application user identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the ApplicationUser who voted.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}
