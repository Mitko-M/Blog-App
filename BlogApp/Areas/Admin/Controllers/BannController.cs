using BlogApp.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BannController : Controller
    {
        private readonly IAdminService _adminService;
        public BannController(
            IAdminService adminService
            )
        {
            _adminService = adminService;
        }
        [HttpPost]
        public async Task<IActionResult> Bann(string userName)
        {
            try
            {
                await _adminService.Bann(userName);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UnBann(string userName)
        {
            try
            {
                await _adminService.UnBann(userName);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
