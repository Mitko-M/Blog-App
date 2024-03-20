using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class CommentController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
