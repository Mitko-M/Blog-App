using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Comment;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult AddComment(CommentFormModel model)
        {
            return View();
        }
    }
}
