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
        Task<IEnumerable<AllPostsViewModel>> GetAllPostsAsync();

        Task AddPostAsync(AddPostFormModel postModel, string userId);

        Task<PostViewModel> FindByIdAsync(int id);

        Task UpdatePostAsync(AddPostFormModel postModel, Post postToEdit);

        Task DeletePostAsync(Post post);
    }
}
