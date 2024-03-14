﻿using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Post;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System.Security.Claims;

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
        public IActionResult Create()
        {
            var model = _postService.GetPostFormModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddPostFormModel model)
        {
            //getting the categories and tags again because they aren't binded...i don't know why
            model.Categories = _postService.GetPostFormModelAsync().Categories;
            model.Tags = _postService.GetPostFormModelAsync().Tags;

            var httpRequestBodyValuesCats = HttpContext.Request.Form["category.IsSelected"];
            var httpRequestBodyValuesTags = HttpContext.Request.Form["tag.IsSelected"];

            List<int> selectedCats = _postService.RequestSelectionToList(httpRequestBodyValuesCats);
            List<int> selectedTags = _postService.RequestSelectionToList(httpRequestBodyValuesTags);


            foreach (var item in selectedCats)
            {
                model.Categories.Find(c => c.Id == item).IsSelected = true;
            }

            foreach (var item in selectedTags)
            {
                model.Tags.Find(t => t.Id == item).IsSelected = true;
            }

            await _postService.AddPostAsync(model, User.Id());

            return RedirectToAction("All", "Home");
        }
    }
}