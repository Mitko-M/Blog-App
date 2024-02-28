using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Infrastructure.Data.Models
{
    public class Tag
    {
        [Key]
        [Comment("Tag identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Tag name")]
        [StringLength(TagNameMax, MinimumLength = TagNameMin)]
        public string Name { get; set; } = string.Empty;

        public ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();
    }
}
