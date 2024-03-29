using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Post;
using BlogApp.Core.Models.Report;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Controllers
{
    public class PostController : BaseController
    {
        private readonly ILogger<PostController> _logger;
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
            model.Categories = await _categoryService.GetCategoriesWithIsSelected();
            model.Tags = await _tagService.GetTagsWithIsSelected();

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

            try
            {
                await _postService.AddPostAsync(model, User.Id());
            }
            catch (ArgumentException ex)
            {
                _logger.LogCritical("Exception on post creating", ex.Message);
                return StatusCode(500);
            }

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
                return NotFound();
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
                return NotFound();
            }

            if (post.UserId != User.Id())
            {
                return Unauthorized();
            }

            //TODO: Make a model binder for the categories and tags since i used ul
            //and it can't have the asp-for attribute
            model.Categories = await _categoryService.GetCategoriesWithIsSelected();
            model.Tags = await _tagService.GetTagsWithIsSelected();

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

            try
            {
                await _postService.UpdatePostAsync(post, model);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500);
                _logger.LogCritical("Exception on post updationg", ex.Message);
            }

            return RedirectToAction("All", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetPostById(id);

            if (post == null)
            {
                return NotFound();
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
                return NotFound();
            }

            if (post.UserId != User.Id())
            {
                return Unauthorized();
            }

            try
            {
                await _postService.DeletePostAsync(post);
            }
            catch (ArgumentException ex)
            {
                _logger.LogCritical("Exception on post deleting", ex.Message);
                return StatusCode(500);
            }

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

        [HttpGet]
        public async Task<IActionResult> Report(int id)
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

            var model = new PostReportViewModel();

            TempData["PostId"] = id;
            TempData["UserId"] = User.Id();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Report(PostReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int postId = int.Parse(TempData["PostId"].ToString());
            string userId = TempData["UserId"].ToString();

            var post = await _postService.GetPostById(postId);

            if (post == null)
            {
                return NotFound();
            }

            model.PostId = postId;
            model.UserId = userId;

            await _postService.ReportPost(model);

            return RedirectToAction(nameof(Details), "Post", new { id = model.PostId });
        }
    }
}
