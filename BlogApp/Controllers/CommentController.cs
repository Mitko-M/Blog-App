﻿using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Comment;
using BlogApp.Core.Models.Post;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class CommentController : BaseController
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        public CommentController(IPostService postService
            , ICommentService commentService)
        {
            _postService = postService;
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentFormModel model)
        {
            model.UserId = User.Id();

            //make custom model binder or hidden input when you have time
            var post = await _postService.GetPostById(model.PostId);

            var postModel = _postService.GetPostDetailsViewModel(post);

            string title = postModel.GetPostTitleInformation();

            await _commentService.AddCommentAsync(model);

            return RedirectToAction("Details", "Post", new { id = model.PostId, title = title});
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> LoadComments([FromQuery] int postId)
        {
            var comments = await _commentService.LoadCommentsAsync(postId);

            var model = comments.ToList();

            return PartialView("_CommentPartial", model);
        }
    }
}
