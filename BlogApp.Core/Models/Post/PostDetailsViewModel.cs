using BlogApp.Core.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Post
{
    public class PostDetailsViewModel : PostsViewModel
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Favorites { get; set; }

        public bool LoadComments { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
