using BlogApp.Core.Contracts;
using BlogApp.Core.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Areas.User.Controllers
{
    public class ManageController : UserBaseController
    {
        private IUserService _userService;
        public ManageController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserById(User.Id());

            var model = new ApplicationUserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ApplicationUserViewModel model)
        {
            try
            {
                await _userService.UpdateUserData(model, User.Id());
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return RedirectToAction(nameof(Index), "Manage");
        }
    }
}
