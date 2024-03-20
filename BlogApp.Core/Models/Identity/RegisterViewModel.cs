using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Core.Models.Identity
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = RequiredError)]
        [Display(Name = "First Name")]
        [StringLength(FirstNameMax, MinimumLength = FirstNameMin, ErrorMessage = InputError)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [Display(Name = "Last Name")]
        [StringLength(LastNameMax, MinimumLength = LastNameMin, ErrorMessage = InputError)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(UserNameMax, MinimumLength = UserNameMin, ErrorMessage = InputError)]
        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [EmailAddress]
        [StringLength(EmailMax, MinimumLength = EmailMin, ErrorMessage = InputError)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = RequiredError)]
        [StringLength(PassMax, MinimumLength = PassMin)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
