using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a user in the BlogApp system.
    /// 
    /// Extends ASP.NET Core Identity's IdentityUser to add custom profile information and site-specific properties.
    /// ApplicationUser is the primary entity for authentication and user management.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The user's first name.
        /// Length: 1-50 characters (defined in ValidationConstants)
        /// Required for all users.
        /// </summary>
        [Required]
        [StringLength(FirstNameMax, MinimumLength = FirstNameMin)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The user's last name.
        /// Length: 1-50 characters (defined in ValidationConstants)
        /// Required for all users.
        /// </summary>
        [Required]
        [StringLength(LastNameMax, MinimumLength = LastNameMin)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Flag indicating if the user has been banned from the platform.
        /// When true, the user cannot create posts or comments, though existing content remains.
        /// Administrators can ban/unban users through the admin dashboard.
        /// </summary>
        public bool Banned { get; set; }

        /// <summary>
        /// Collection of warnings issued to this user.
        /// Users receive warnings for policy violations (inappropriate content, spam, etc.).
        /// Accumulated warnings may lead to user banning.
        /// </summary>
        public ICollection<Warning> Warnings { get; set; } = new List<Warning>();

        /// <summary>
        /// Collection of contact form entries submitted by this user.
        /// Tracks all contact form submissions for administrative review.
        /// </summary>
        public ICollection<ContactFormEntry> ContactFormEntries { get; set; } = new List<ContactFormEntry>();
    }
}
