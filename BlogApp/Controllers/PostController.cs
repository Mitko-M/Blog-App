using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Comment;
using BlogApp.Core.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Controllers
{
    public class PostController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public PostController(
            ILogger<PostController> logger,
            IPostService postService,
            ICategoryService categoryService,
            ITagService tagService)
        {
            _logger = logger;
            _postService = postService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _postService.GetPostFormModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddPostFormModel model)
        {
            //TODO: Make a model binder for the categories and tags since i used ul
            //and it can't have the asp-for attribute
            model.Categories = await _postService.GetCategoriesWithIsSelected();
            model.Tags = await _postService.GetTagsWithIsSelected();

            var httpRequestBodyValuesCats = HttpContext.Request.Form["category.IsSelected"];
            var httpRequestBodyValuesTags = HttpContext.Request.Form["tag.IsSelected"];

            List<int> selectedCats = _postService.RequestSelectionToList(httpRequestBodyValuesCats);
            List<int> selectedTags = _postService.RequestSelectionToList(httpRequestBodyValuesTags);


            foreach (var item in selectedCats)
            {
                model.Categories.ToList().Find(c => c.Id == item).IsSelected = true;
            }

            foreach (var item in selectedTags)
            {
                model.Tags.ToList().Find(t => t.Id == item).IsSelected = true;
            }

            if (!model.Categories.Any(c => c.IsSelected == true))
            {
                ModelState.AddModelError("Post Category", "A category wasn't chosen");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _postService.AddPostAsync(model, User.Id());

            return RedirectToAction("All", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!(User?.Identity?.IsAuthenticated) ?? false)
            {
                return RedirectToAction("Login", "User");
            }

            var post = await _postService.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            var model = _postService.GetPostDetailsViewModel(post);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _postService.GetPostToEditAsync(id);

            if (model == null)
            {
                return BadRequest();
            }

            if (model.UserId != User.Id())
            {
                return Unauthorized();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddPostFormModel model)
        {
            var post = await _postService.GetPostById(id);

            if (post == null)
            {
                return BadRequest();
            }

            if (post.UserId != User.Id())
            {
                return Unauthorized();
            }

            //TODO: Make a model binder for the categories and tags since i used ul
            //and it can't have the asp-for attribute
            model.Categories = await _postService.GetCategoriesWithIsSelected();
            model.Tags = await _postService.GetTagsWithIsSelected();

            var httpRequestBodyValuesCats = HttpContext.Request.Form["category.IsSelected"];
            var httpRequestBodyValuesTags = HttpContext.Request.Form["tag.IsSelected"];

            List<int> selectedCats = _postService.RequestSelectionToList(httpRequestBodyValuesCats);
            List<int> selectedTags = _postService.RequestSelectionToList(httpRequestBodyValuesTags);


            foreach (var item in selectedCats)
            {
                model.Categories.ToList().Find(c => c.Id == item).IsSelected = true;
            }

            foreach (var item in selectedTags)
            {
                model.Tags.ToList().Find(t => t.Id == item).IsSelected = true;
            }

            if (!model.Categories.Any(c => c.IsSelected == true))
            {
                ModelState.AddModelError("Post Category", "A category wasn't chosen");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _postService.UpdatePostAsync(post, model);

            return RedirectToAction("All", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetPostById(id);

            if (post == null)
            {
                return BadRequest();
            }

            if (post.UserId != User.Id())
            {
                return Unauthorized();
            }

            var model = new DeletePostViewModel()
            {
                Id = id,
                Title = post.Title,
                CreatedOn = post.CreatedOn.ToString(PostDateFormat)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _postService.GetPostById(id);

            if (post == null)
            {
                return BadRequest();
            }

            if (post.UserId != User.Id())
            {
                return Unauthorized();
            }

            await _postService.DeletePostAsync(post);

            return RedirectToAction("All", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Mine([FromQuery] AllPostsQueryModel model)
        {
            var posts = await _postService.GetMinePostsAsync(
                User.Id(),
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
            model.MinePosts = true;

            if (model.CurrentPage > Math.Ceiling((double)model.PostsCount / model.PostsPerPage) && model.CurrentPage > 1)
            {
                return BadRequest();
            }

            return View(model);
        }
    }
}
