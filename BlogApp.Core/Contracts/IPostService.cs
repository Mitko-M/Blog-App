using BlogApp.Core.Enumerations;
using BlogApp.Core.Models;
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
        /// <summary>
        /// Returns all posts asynchronously base on the filters
        /// </summary>
        /// <returns>An AllPostsQueryModel</returns>
        Task<PostQueryServiceModel> GetAllPostsAsync(
            string? tagName = null,
            string? categoryName = null,
            PostSorting sorting = PostSorting.None,
            int currentPage = 1,
            int postsPerPage = 1,
            string searchTerm = null
            );

        /// <summary>
        /// Adding a post asynchronously
        /// </summary>
        /// <param name="postModel"> Post's name</param>
        /// <param name="userId">Author's identifier</param>
        /// <returns>A task</returns>
        Task AddPostAsync(AddPostFormModel postModel, string userId);

        /// <summary>
        /// A method for taking values from the HTTP request's body
        /// </summary>
        /// <param name="values">All values from the HTTP request body</param>
        /// <returns>A list of int</returns>
        List<int> RequestSelectionToList(string values);

        /// <summary>
        /// Creating a post model to be filled in a form
        /// </summary>
        /// <returns>AddPostFormModel</returns>
        AddPostFormModel GetPostFormModel();

        /// <summary>
        /// A method for taking all tags from the database and then parsing them to a model
        /// </summary>
        /// <returns>A list with PostTagModel</returns>
        List<PostTagModel> GetTags();

        /// <summary>
        /// A method for taking all categories from the database and then parsing them to a model
        /// </summary>
        /// <returns>A list with PostCategoryModel</returns>
        List<PostCategoryModel> GetCategories();

        /// <summary>
        /// Getting a post by it's Id
        /// </summary>
        /// <param name="id">Post's identifier</param>
        /// <returns>A post entity</returns>
        Task<Post?> GetPostById(int id);
        
        //TODO: Add the rest CRUD operations
    }
}
