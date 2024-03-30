using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(FirstNameMax, MinimumLength = FirstNameMin)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(LastNameMax, MinimumLength = LastNameMin)]
        public string LastName { get; set; } = string.Empty;

        public bool Banned { get; set; }

        public ICollection<Warning> Warnings { get; set; }
        public ICollection<ContactFormEntry> ContactFormEntries { get; set; }
    }
}
