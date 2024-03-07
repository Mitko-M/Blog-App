using BlogApp.Core.Contracts;
using BlogApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;

        public HomeController(
            ILogger<HomeController> logger,
            IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await _postService.GetAllPostsAsync();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
