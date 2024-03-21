using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    public class LikeDislike
    {
        [Key]
        [Comment("LikeDislike identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Boolean which determines whether something is liked or not")]
        public bool Liked { get; set; }

        [Required]
        [Comment("Post identifier")]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        [Required]
        [Comment("Application user identifier")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}
