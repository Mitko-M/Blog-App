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
        /// Returns all posts asynchronously based on the filters given with the query string in the URL
        /// </summary>
        /// <returns>An AllPostsQueryModel containing every post mathing the given parameters</returns>
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
        Task<AddPostFormModel> GetPostFormModel();

        /// <summary>
        /// A method for taking all tags from the database and then parsing them to a model
        /// </summary>
        /// <returns>A list with PostTagModel</returns>
        Task<IEnumerable<PostTagFormModel>> GetTagsWithIsSelected();

        /// <summary>
        /// A method for taking all categories from the database and then parsing them to a model
        /// </summary>
        /// <returns>A list with PostCategoryModel</returns>
        Task<IEnumerable<PostCategoryFormModel>> GetCategoriesWithIsSelected();

        /// <summary>
        /// Getting a post by it's Id
        /// </summary>
        /// <param name="id">Post's identifier</param>
        /// <returns>A post entity</returns>
        Task<Post?> GetPostById(int id);

        /// <summary>
        /// Finds a post by given Id and returns it as model for editing
        /// </summary>
        /// <param name="id">Post's identifier</param>
        /// <returns>An AddPostFormModel to be edited in a form</returns>
        Task<AddPostFormModel> GetPostToEditAsync(int id);

        /// <summary>
        /// A method for updating the post in the database
        /// </summary>
        /// <param name="post">The database object post</param>
        /// <param name="model">The model containing all change data</param>
        /// <returns></returns>
        Task UpdatePostAsync(Post post, AddPostFormModel model);

        /// <summary>
        /// A method for deleting a post asynchronously
        /// </summary>
        /// <param name="post">Post entity</param>
        /// <returns></returns>
        Task DeletePostAsync(Post post);
    }
}
