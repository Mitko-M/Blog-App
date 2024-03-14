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

        List<int> RequestSelectionToList(string values);

        AddPostFormModel GetPostFormModelAsync();
        
        //TODO: Add the rest CRUD operations
    }
}
