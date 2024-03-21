using BlogApp.Core.Enumerations;
using BlogApp.Core.Models.Post;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class SortAndFilterController : BaseController
    {
        public IActionResult Index([FromQuery] AllPostsQueryModel model)
        {
            string controller = model.MinePosts ? "Post" : "Home";
            string action = model.MinePosts ? "Mine" : "All";

            model.CategoryName = null;
            model.TagName = null;
            model.PostSorting = PostSorting.None;

            return RedirectToAction(action, controller, model);
        }
    }
}
