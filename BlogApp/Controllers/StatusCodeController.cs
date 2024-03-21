using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [AllowAnonymous]
    public class StatusCodeController : BaseController
    {
        [Route("StatusCode/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            return View(statusCode.ToString());
        }
    }
}
