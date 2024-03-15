using BlogApp.Core.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Post
{
    public class PostDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public string UpdatedOn { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public IEnumerable<string> Categories { get; set; } = new List<string>();
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Favorites { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
