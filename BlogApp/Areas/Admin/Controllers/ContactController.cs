using BlogApp.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ILogger _logger;
        public ContactController(
            IAdminService adminService,
            ILogger<ContactController> logger
            )
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var contactForms = await _adminService.GetAllContactFormsAsync();

            return View(contactForms);
        }

        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            var contactForm = await _adminService.GetContactFormById(id);

            if (contactForm == null)
            {
                return NotFound();
            }

            return View(contactForm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _adminService.DeleteContactFormEntry(id);
            }
            catch (ArgumentException)
            {
                _logger.LogCritical($"Something happened while deleting a contact form with id {id}");
                return StatusCode(500);
            }

            return RedirectToAction(nameof(All), "Contact");
        }
    }
}
