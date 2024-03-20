﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
