using BlogApp.Core.Contracts;
using BlogApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class ContactController : BaseController
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;
        public ContactController(
            ILogger<ContactController> logger,
            IContactService contactService
            )
        {
            _logger = logger;
            _contactService = contactService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = new ContactViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            model.UserId = User.Id();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _contactService.SubmitContactForm(model);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

            return View(nameof(SuccessfullySentForm));
        }

        [HttpGet]
        public IActionResult SuccessfullySentForm()
        {
            return View();
        }
    }
}
