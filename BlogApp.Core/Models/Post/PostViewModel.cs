using BlogApp.Infrastructure.Data.Models;
using BlogApp.InfrastructureData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Post
{
    //think it over something isn't right...i don't know what
    public class PostViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
     
        public string ShortDescription { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<PostCategory> PostsCategories { get; set; } = new HashSet<PostCategory>();

        public ICollection<PostTag> PostsTags { get; set; } = new HashSet<PostTag>();

        public ICollection<LikeDislike> LikesDislikes { get; set; } = new HashSet<LikeDislike>();

        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
    }
}
