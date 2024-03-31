using BlogApp.Core.Contracts;
using BlogApp.Core.Models;
using BlogApp.Core.Models.Identity;
using BlogApp.Core.Models.Report;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Policy;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAdminService _adminService;
        private readonly IPostService _postService;
        public AdminController(
            ILogger<AdminController> logger,
            IAdminService adminService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPostService postService)
        {
            _logger = logger;
            _adminService = adminService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _postService = postService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await _adminService.GetAllUsersAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DashboardWithAdmins()
        {
            var model = await _adminService.GetAdminsAsync();

            return View("Dashboard", model);
        }

        [HttpGet]
        public async Task<IActionResult> DashboardWithUsers()
        {
            var model = await _adminService.GetUsersAsync();

            return View("Dashboard", model);
        }

        [HttpGet]
        public IActionResult AddAdmin()
        {
            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                try
                {
                    await AddUserToAdminRole(user.Id);
                }
                catch (ArgumentException)
                {
                    return StatusCode(500);
                }

                return RedirectToAction(nameof(Dashboard), "Admin");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var reports = await _adminService.GetAllReportsAsync();

            return View(reports);
        }

        [HttpGet]
        public async Task<IActionResult> PreviewReport(int id)
        {
            var report = await _adminService.GetReportById(id);
            var post = await _postService.GetPostById(report.PostId);

            string postOwner = post.UserId;

            if (report == null)
            {
                return NotFound();
            }

            string serializedReport = JsonConvert.SerializeObject(report);

            TempData["ReportModel"] = serializedReport;
            TempData["PostOwner"] = postOwner;

            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int reportId)
        {
            try
            {
                await _adminService.DeleteReport(reportId);
            }
            catch (ArgumentException)
            {
                _logger.LogCritical($"Something happened while deleting a report with id {reportId}");
                return StatusCode(500);
            }

            var reports = await _adminService.GetAllReportsAsync();

            return View(nameof(Reports), reports);
        }

        [HttpPost]
        public async Task<IActionResult> WarnUser()
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
                return NotFound();
            }

            var reports = await _adminService.GetAllReportsAsync();

            return RedirectToAction(nameof(Reports), reports);
        }

        private async Task AddUserToAdminRole(string userId)
        {
            var roleName = "Admin";
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (roleExists)
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    if (!await _userManager.IsInRoleAsync(user, roleName))
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }
                    else
                    {
                        _logger.LogWarning($"User is already in role {roleName}");
                    }
                }
                else
                {
                    _logger.LogError("User wasn't found while trying to add role admin to a user");
                    throw new ArgumentException("User wasn't found in the database");
                }
            }
            else
            {
                _logger.LogCritical("Admin role doesn't exist. Tried to add the role to a user");

                throw new ArgumentException("Admin role doesn't exist in the database");
            }
        }
    }
}
