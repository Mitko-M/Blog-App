using BlogApp.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        public AdminController(
            ILogger<AdminController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await _userService.GetAllUsersAsync();

            return View(model);
        }

        public async Task<IActionResult> DashboardWithAdmins()
        {
            var model = await _userService.GetAdminsAsync();

            return View("Dashboard", model);
        }
    }
}
