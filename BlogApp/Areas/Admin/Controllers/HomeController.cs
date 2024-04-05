using BlogApp.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IAdminService _adminService;
        public HomeController(
            IAdminService adminService
            )
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            var model = await _adminService.GetAllUsersAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var model = await _adminService.GetUsersAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Admins()
        {
            var model = await _adminService.GetAdminsAsync();

            return View(model);
        }
    }
}
