using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Areas.User.Controllers
{
    public class AccessController : UserBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
