using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a warning issued to a user for policy violations.
    /// 
    /// Administrators issue warnings when users violate community guidelines.
    /// Warnings are associated with a specific hidden post that triggered the warning.
    /// Accumulated warnings may lead to user suspension or banning.
    /// </summary>
    public class Warning
    {
        /// <summary>
        /// Primary key identifier for the warning.
        /// </summary>
        [Comment("Warning identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the ApplicationUser who received this warning.
        /// </summary>
        [Comment("Warned user's identifier")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the ApplicationUser who received this warning.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// The ID of the post that was hidden due to policy violation.
        /// This is the post that triggered the warning to be issued.
        /// The post remains hidden until the user deletes it or addresses the violation.
        /// </summary>
        [Comment("A post which was reported then checked hence the owner was warned and then the post hidden and left for the owner to delete it")]
        public int HiddenPostId { get; set; }

        /// <summary>
        /// The specific reason for issuing this warning.
        /// Explains the policy violation (e.g., "Inappropriate language", "Spam", "Harassment").
        /// Helps users understand what they did wrong and how to avoid future violations.
        /// </summary>
        [Comment("The reason for adding a warning")]
        public string WarningReason { get; set; } = string.Empty;
    }
}
