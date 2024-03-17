using BlogApp.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Post
{
    public class AllPostsQueryModel
    {
        public string TagName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public PostSorting PostSorting { get; set; } = PostSorting.None;
        public int CurrentPage { get; set; } = 1;
        public int PostsPerPage { get; } = 4;
        public string SearchTerm { get; set; } = null!;
        public int PostsCount { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public IEnumerable<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
        public IEnumerable<PostsViewModel> Posts { get; set; } = new List<PostsViewModel>();
    }
}
