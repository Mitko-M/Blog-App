using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class UserBaseController : Controller
    {

    }
}
