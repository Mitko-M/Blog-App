using BlogApp.Core.Models.Identity;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            ILogger<UserController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Home");
            }

            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                await AddUserToRole(user.Id);

                try
                {
                    await AddUserToRole(user.Id);
                }
                catch (ArgumentException)
                {
                    return StatusCode(500);
                }


                return RedirectToAction("Login", "User");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

        private async Task AddUserToRole(string userId)
        {
            var roleName = "User";
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
                    _logger.LogError("User wasn't found");
                    throw new ArgumentException("User wasn't found in the database");
                }
            }
            else
            {
                _logger.LogCritical("User role doesn't exist");

                throw new ArgumentException("User role doesn't exist in the database");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("All", "Home");
            }

            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("All", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("All", "Home");
        }
    }
}
