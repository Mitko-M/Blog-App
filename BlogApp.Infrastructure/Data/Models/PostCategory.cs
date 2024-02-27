using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Infrastructure.Data.Models
{
    public class PostCategory
    {
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
    }
}
