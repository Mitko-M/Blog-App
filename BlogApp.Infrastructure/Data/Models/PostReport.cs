using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a report filed against a blog post for policy violations.
    /// 
    /// Users can report posts for inappropriate content, spam, or other violations.
    /// Administrators review reports and can take action (hide post, issue warnings, ban user).
    /// Reports help maintain community standards and content quality.
    /// </summary>
    public class PostReport
    {
        /// <summary>
        /// Primary key identifier for the report.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The reason or description for reporting this post.
        /// Details the policy violation or issue reported by the user.
        /// </summary>
        [Required]
        public string ReportContent { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key to the ApplicationUser who filed this report.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the ApplicationUser who filed this report.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Foreign key to the Post being reported.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Navigation property to the Post being reported.
        /// </summary>
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;
    }
}
