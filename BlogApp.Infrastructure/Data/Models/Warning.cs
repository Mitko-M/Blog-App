using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Data.Models
{
    public class Warning
    {
        [Comment("Warning identifier")]
        public int Id { get; set; }

        [Comment("Warned user's identifier")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [Comment("A post which was reported then checked hence the owner was warned and then the post hidden and left for the owner to delete it")]
        public int HiddenPostId { get; set; }

        [Comment("The reason for adding a warning")]
        public string WarningReason { get; set; } = string.Empty;
    }
}
