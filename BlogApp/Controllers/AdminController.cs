﻿using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAdminService _userService;
        public AdminController(
            ILogger<AdminController> logger,
            IAdminService userService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userService = userService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await _userService.GetAllUsersAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DashboardWithAdmins()
        {
            var model = await _userService.GetAdminsAsync();

            return View("Dashboard", model);
        }

        [HttpGet]
        public async Task<IActionResult> DashboardWithUsers()
        {
            var model = await _userService.GetUsersAsync();

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

        [HttpPost]
        public async Task<IActionResult> PromoteToAdmin(string userId)
        {
            try
            {
                await AddUserToAdminRole(userId);
            }
            catch (ArgumentException)
            {
                return StatusCode(500);
            }

            return RedirectToAction(nameof(Dashboard), "Admin");
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
