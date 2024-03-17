using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Comment;
using BlogApp.Core.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Controllers
{
    public class PostController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IPostService _postService;

        public PostController(
            ILogger<PostController> logger,
            IPostService postService)
        {
            _logger = logger;
            _postService = postService;
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
                return RedirectToAction("Login", "Account");
            }

            var post = await _postService.GetPostById(id);

            if (post == null)
            {
                return BadRequest();
            }

            var categories = post.PostsCategories
                .Select(pc => pc.Category.Name)
                .ToList();

            var tags = post.PostsTags
                .Select(pt => pt.Tag.Name)
                .ToList();

            int likes = post.LikesDislikes
                .Where(ld => ld.Liked)
                .Count();

            int dislikes = post.LikesDislikes
                .Where(ld => !ld.Liked)
                .Count();

            int favorites = post.Favorites.Count();

            var comments = post.Comments
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserId = c.UserId,
                    PostId = c.PostId,
                })
                .ToList();

            PostDetailsViewModel model = new PostDetailsViewModel()
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                ShortDescription = post.ShortDescription,
                CreatedOn = post.CreatedOn.ToString(PostDateFormat),
                UpdatedOn = post.UpdatedOn.ToString(PostDateFormat),
                UserName = post.User.UserName,
                Categories = categories,
                Tags = tags,
                Likes = likes,
                Dislikes = dislikes,
                Favorites = favorites,
                Comments = comments
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _postService.GetPostToEditAsync(id);

            if (model == null)
            {
                return RedirectToAction("All", "Home");
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
    }
}
