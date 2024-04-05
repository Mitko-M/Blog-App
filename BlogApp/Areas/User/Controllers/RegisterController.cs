using BlogApp.Core.Models.Identity;
using BlogApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Areas.User.Controllers
{
    [AllowAnonymous]
    public class RegisterController : UserBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            RegisterViewModel model = new RegisterViewModel();

            return View(model);
        }

        public async Task<IActionResult> Index(RegisterViewModel model)
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
                UserName = model.UserName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                try
                {
                    await AddUserToRole(user.Id);
                }
                catch (ArgumentException)
                {
                    return StatusCode(500);
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code.ToString(), error.Description);
            }

            return RedirectToAction("Index", "Login");
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
                }
                else
                {
                    throw new ArgumentException("User wasn't found in the database");
                }
            }
            else
            {
                throw new ArgumentException("User role doesn't exist in the database");
            }
        }
    }
}
