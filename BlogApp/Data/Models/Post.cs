using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BlogApp.Common.ValidationConstants;

namespace BlogApp.Data.Models
{
    public class Post
    {
        [Key]
        [Comment("Post identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Post title")]
        [StringLength(PostTitleMax, MinimumLength = PostTitleMin)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Comment("Post content")]
        [StringLength(PostContentMax, MinimumLength = PostContentMin)]
        public string Content { get; set; } = string.Empty;

        //it can be shown as an intro on preview 
        [Required]
        [Comment("Post short description acting as intro to engage the reader to read more")]
        [StringLength(PostShortDescriptionMax, MinimumLength = PostShortDescriptionMin)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        [Comment("Post creation date")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("Post last update date")]
        public DateTime UpdatedOn { get; set;}

        [Required]
        [Comment("Application user and post creator identifier")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<PostCategory> PostsCategories { get; set; } = new HashSet<PostCategory>();

        public ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();

        public ICollection<LikeDislike> LikesDislikes { get; set; } = new HashSet<LikeDislike>();

        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
    }
}
