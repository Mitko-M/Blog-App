using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static BlogApp.Common.ValidationConstants;

namespace BlogApp.Data.Models
{
    public class Category
    {
        [Key]
        [Comment("Category identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Category name")]
        [StringLength(CategoryNameMax, MinimumLength = CategoryNameMin)]
        public string Name { get; set; } = string.Empty;

        public ICollection<PostCategory> PostsCategories { get; set; } = new HashSet<PostCategory>();
    }
}
