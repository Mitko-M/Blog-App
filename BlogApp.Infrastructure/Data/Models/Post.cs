using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a blog post in the system.
    /// 
    /// A post is the core content entity that users create and publish. Posts can:
    /// - Be organized into categories and tagged with tags
    /// - Receive comments from other users
    /// - Be liked/disliked by users
    /// - Be reported for inappropriate content
    /// - Be marked as favorites by users
    /// 
    /// Validation Rules:
    /// - Title: 5-50 characters (required)
    /// - Content: 10-5000 characters (required, HTML allowed)
    /// - ShortDescription: 10-150 characters (required, preview text)
    /// - CreatedOn/UpdatedOn: Auto-managed timestamps
    /// - UserId: Foreign key to ApplicationUser (post author)
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Primary key identifier for the post.
        /// </summary>
        [Key]
        [Comment("Post identifier")]
        public int Id { get; set; }

        /// <summary>
        /// The post title displayed on listing pages and post details page.
        /// Length: 5-50 characters (defined in ValidationConstants)
        /// </summary>
        [Required]
        [Comment("Post title")]
        [StringLength(PostTitleMax, MinimumLength = PostTitleMin)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The main post content in HTML format.
        /// Can contain rich text formatting from TinyMCE editor.
        /// HTML is sanitized to prevent XSS attacks.
        /// Length: 10-5000 characters (defined in ValidationConstants)
        /// </summary>
        [Required]
        [Comment("Post content")]
        [StringLength(PostContentMax, MinimumLength = PostContentMin)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// A short preview/excerpt of the post displayed on listing pages.
        /// Used to engage readers and encourage them to read the full post.
        /// Length: 10-150 characters (defined in ValidationConstants)
        /// </summary>
        [Required]
        [Comment("Post short description acting as intro to engage the reader to read more")]
        [StringLength(PostShortDescriptionMax, MinimumLength = PostShortDescriptionMin)]
        public string ShortDescription { get; set; } = string.Empty;

        /// <summary>
        /// The date and time when the post was created.
        /// Set automatically when post is first created.
        /// </summary>
        [Required]
        [Comment("Post creation date")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The date and time when the post was last modified.
        /// Updated whenever the post or its associations (categories/tags) change.
        /// </summary>
        [Required]
        [Comment("Post last update date")]
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Foreign key reference to ApplicationUser (the post author).
        /// Every post must have an author.
        /// </summary>
        [Required]
        [Comment("Application user and post creator identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the post author (ApplicationUser).
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Flag indicating whether the post is hidden from public view.
        /// Hidden posts can only be viewed by admins or the author.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Collection of comments on this post.
        /// Users can leave comments to discuss the post content.
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// Junction collection for Post-Category many-to-many relationship.
        /// Allows a post to belong to multiple categories.
        /// </summary>
        public ICollection<PostCategory> PostsCategories { get; set; } = new HashSet<PostCategory>();

        /// <summary>
        /// Junction collection for Post-Tag many-to-many relationship.
        /// Allows a post to have multiple tags for fine-grained categorization.
        /// </summary>
        public ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();

        /// <summary>
        /// Collection of likes and dislikes on this post.
        /// Each user can like or dislike a post once.
        /// </summary>
        public ICollection<LikeDislike> LikesDislikes { get; set; } = new HashSet<LikeDislike>();

        /// <summary>
        /// Collection of users who marked this post as a favorite.
        /// Users can bookmark posts for later reading.
        /// </summary>
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();

        /// <summary>
        /// Collection of user reports for this post.
        /// Reports are used for moderation to flag inappropriate content.
        /// </summary>
        public ICollection<PostReport> PostReports { get; set; } = new List<PostReport>();
    }
}
