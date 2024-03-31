using System.ComponentModel.DataAnnotations;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Core.Models
{
    public class ContactViewModel
    {
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(ContactFormNameMax, MinimumLength = ContactFormNameMin, ErrorMessage = InputError)]
        public string Name { get; set; }

        [Required(ErrorMessage = RequiredError)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = RequiredError)]
        [StringLength(ContactFormSubjectMax, MinimumLength = ContactFormSubjectMin, ErrorMessage = InputError)]
        public string Subject { get; set; }

        [Required(ErrorMessage = RequiredError)]
        [StringLength(ContactFormMessageMax, MinimumLength = ContactFormMessageMin, ErrorMessage = InputError)]
        public string Message { get; set; }
    }
}
