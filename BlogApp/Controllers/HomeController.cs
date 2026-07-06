using BlogApp.Core.Contracts;
using BlogApp.Core.Models;
using BlogApp.Core.Models.Post;
using BlogApp.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    /// <summary>
    /// Main homepage controller handling public-facing views and post listings.
    /// 
    /// Provides endpoints for:
    /// - Displaying all posts with filtering, searching, and sorting
    /// - Handling error pages
    /// - Supporting anonymous user access
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public HomeController(
            ILogger<HomeController> logger,
            IPostService postService,
            ICategoryService categoryService,
            ITagService tagService)
        {
            _logger = logger;
            _postService = postService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        /// <summary>
        /// GET: / or /Home/All
        /// Displays all blog posts with support for filtering, searching, and sorting.
        /// 
        /// Query Parameters:
        /// - tagName: Filter posts by tag name (optional)
        /// - categoryName: Filter posts by category name (optional)
        /// - postSorting: Sort order (Newest, Oldest, MostLiked, etc.) (optional)
        /// - currentPage: Pagination page number (default: 1)
        /// - postsPerPage: Posts per page (default: configured value)
        /// - searchTerm: Full-text search term (optional)
        /// 
        /// Returns: View with AllPostsQueryModel containing filtered posts and categories/tags
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> All([FromQuery]AllPostsQueryModel model)
        {
            var posts = await _postService.GetAllPostsAsync(
                model.TagName,
                model.CategoryName,
                model.PostSorting,
                model.CurrentPage,
                model.PostsPerPage,
                model.SearchTerm);

            var categories = await _categoryService.GetCategoriesAsync();
            var tags = await _tagService.GetTagsAsync();

            model.PostsCount = posts.PostsCount;
            model.Posts = posts.Posts;
            model.Categories = categories;
            model.Tags = tags;
            model.MinePosts = false;

            if (model.CurrentPage > Math.Ceiling((double)model.PostsCount / model.PostsPerPage) && model.CurrentPage > 1)
            {
                return BadRequest();
            }

            return View(model);
        }

        /// <summary>
        /// GET: /Home/Error
        /// Displays error page with request information for debugging.
        /// 
        /// Returns: Error view with RequestId for tracking
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
