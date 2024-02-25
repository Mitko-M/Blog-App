using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static BlogApp.Common.ValidationConstants;

namespace BlogApp.Data.Models
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

        public ICollection<PostTag> PostsCategories { get; set; } = new HashSet<PostTag>();
    }
}
