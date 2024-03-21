using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int PostId { get; set; }
        public DateTime CommentUploadDate { get; set; }
        public IEnumerable<CommentLikeViewModel> CommentsLikes { get; set; } = new List<CommentLikeViewModel>();
    }
}
