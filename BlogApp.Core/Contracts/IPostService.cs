using BlogApp.Core.Enumerations;
using BlogApp.Core.Models.Post;
using BlogApp.Core.Models.Report;
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
        /// A method for getting all posts mathing a given criteria and are associated with the application user 
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="categoryName">Category name</param>
        /// <param name="sorting">Type of sorting</param>
        /// <param name="currentPage">The current page in the All view</param>
        /// <param name="postsPerPage">How many posts per page should be</param>
        /// <param name="searchTerm">The search term from the search bar</param>
        /// <returns>A PostQueryServiceModel containning all posts for a page and the count</returns>
        Task<PostQueryServiceModel> GetMinePostsAsync(
            string UserId,
            string? tagName = null,
            string? categoryName = null,
            PostSorting sorting = PostSorting.None,
            int currentPage = 1,
            int postsPerPage = 1,
            string searchTerm = null);

        /// <summary>
        /// A method for getting all posts matching a given criteria
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="categoryName">Category name</param>
        /// <param name="sorting">Type of sorting</param>
        /// <param name="currentPage">The current page in the All view</param>
        /// <param name="postsPerPage">How many posts per page should be</param>
        /// <param name="searchTerm">The search term from the search bar</param>
        /// <returns>A PostQueryServiceModel containning all posts for a page and the count</returns>
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

        /// <summary>
        /// Creates and returns the model for the Details view in the post controller
        /// </summary>
        /// <param name="post">The post to convert</param>
        /// <returns></returns>
        PostDetailsViewModel GetPostDetailsViewModel(Post post);

        /// <summary>
        /// Adds a report to a post
        /// </summary>
        /// <returns></returns>
        Task ReportPost(PostReportViewModel report);
    }
}
