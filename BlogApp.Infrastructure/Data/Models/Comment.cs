using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Infrastructure.Data.Models
{
    public class Comment
    {
        [Key]
        [Comment("Comment identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Comment content")]
        [StringLength(CommentContentMax, MinimumLength = CommentContentMin)]
        public string Content { get; set; } = string.Empty;

        [Required]
        [Comment("Application user and commen creator identifier")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        [Required]
        [Comment("Post identifier")]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        [Required]
        public DateTime CommentUploadDate { get; set; }

        public ICollection<CommentLike> CommentsLikes { get; set; } = new List<CommentLike>();
    }
}
