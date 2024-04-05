using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using BlogApp.Core.Models.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IAdminService _adminService;
        public UserController(
            IAdminService adminService
            )
        {
            _adminService = adminService;
        }

        [HttpPost]
        public async Task<IActionResult> Warn()
        {
            string serializedReport = TempData["ReportModel"].ToString();
            string postOwner = TempData["PostOwner"].ToString();

            PostReportsAdminViewModel model = JsonConvert.DeserializeObject<PostReportsAdminViewModel>(serializedReport);

            try
            {
                await _adminService.WarnApplicationUser(model.Id, model.PostId, postOwner);
            }
            catch (ArgumentException)
            {
                return StatusCode(500);
            }

            return RedirectToAction("All", "Report");
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string userName)
        {
            ApplicationUserWithAllDataViewModel user;

            try
            {
                user = await _adminService.ManageUserByUserName(userName);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
