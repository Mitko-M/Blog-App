using BlogApp.Core.Models.Post;
using BlogApp.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Contracts
{
    public interface IPostService
    {
        Task<IEnumerable<AllPostsVideModel>> GetAllPostsAsync();

        Task AddPostAsync(AddPostViewModel postModel, string userId);

        Task<PostViewModel> FindByIdAsync(int id);

        Task UpdatePostAsync(AddPostViewModel postModel, Post postToEdit);

        Task DeletePostAsync(Post post);
    }
}
