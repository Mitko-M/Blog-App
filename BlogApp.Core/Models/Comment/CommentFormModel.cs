using System.ComponentModel.DataAnnotations;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Core.Models.Comment
{
    public class CommentFormModel
    {
        [Required(ErrorMessage = RequiredError)]
        [StringLength(CommentContentMax, MinimumLength = CommentContentMin, ErrorMessage = InputError)]
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int PostId { get; set; }
        public DateTime CommentUploadDate { get; set; }
        public bool Liked { get; set; }
    }
}
