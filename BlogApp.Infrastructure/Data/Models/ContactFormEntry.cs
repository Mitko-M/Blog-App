using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a contact form submission from a user.
    /// 
    /// Users can submit contact form entries to reach out to administrators.
    /// All submissions are stored for admin review and response.
    /// Each entry is associated with the user who submitted it.
    /// </summary>
    public class ContactFormEntry
    {
        /// <summary>
        /// Primary key identifier for the contact form entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the ApplicationUser who submitted this contact form.
        /// Links the submission to the authenticated user.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the ApplicationUser who submitted this form.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// The contact name field from the form.
        /// May be the user's full name or a display name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The email address provided in the contact form.
        /// Used for administrators to respond to the inquiry.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The subject line of the contact form submission.
        /// Brief description of the inquiry topic.
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// The main message content of the contact form.
        /// Detailed inquiry or feedback from the user.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the contact form was submitted.
        /// Set automatically when entry is created.
        /// Used to track submission order and response timing.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
