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

            if (model.CurrentPage > Math.Ceiling((double)model.PostsCount / model.PostsPerPage) && model.CurrentPage > 1)
            {
                return BadRequest();
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
