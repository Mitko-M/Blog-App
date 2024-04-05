using System.ComponentModel.DataAnnotations;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Core.Models.Identity
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Display(Name = "First Name")]
        [Required(ErrorMessage = RequiredError)]
        [StringLength(FirstNameMax, MinimumLength = FirstNameMin, ErrorMessage = InputError)]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = RequiredError)]
        [StringLength(LastNameMax, MinimumLength = LastNameMin, ErrorMessage = InputError)]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Username")]
        [Required(ErrorMessage = RequiredError)]
        [StringLength(UserNameMax, MinimumLength = UserNameMin, ErrorMessage = InputError)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(EmailMax, MinimumLength = EmailMin, ErrorMessage = InputError)]
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int PostCount { get; set; }

        [Display(Name = "Phone Nubmer")]
        [Phone]
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
