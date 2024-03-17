using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Post
{
    public class PostQueryServiceModel
    {
        public int PostsCount { get; set; }
        public IEnumerable<PostsViewModel> Posts { get; set; } = new List<PostsViewModel>();
    }
}
