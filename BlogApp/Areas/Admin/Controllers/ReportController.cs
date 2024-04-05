using BlogApp.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogApp.Areas.Admin.Controllers
{
    public class ReportController : AdminBaseController
    {
        private readonly ILogger _logger;
        private readonly IAdminService _adminService;
        private readonly IPostService _postService;
        public ReportController(
            ILogger<ReportController> logger,
            IAdminService adminService,
            IPostService postService
            )
        {
            _logger = logger;
            _adminService = adminService;
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var reports = await _adminService.GetAllReportsAsync();

            return View(reports);
        }

        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            var report = await _adminService.GetReportById(id);
            var post = await _postService.GetPostById(report.PostId);

            string postOwner = post.UserId;

            if (report == null)
            {
                return NotFound();
            }

            //temp data only accepts objects as json
            //later this temp data is used in WarnUser if the admin decides to warn him/her
            string serializedReport = JsonConvert.SerializeObject(report);

            TempData["ReportModel"] = serializedReport;
            TempData["PostOwner"] = postOwner;

            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _adminService.DeleteReport(id);
            }
            catch (ArgumentException)
            {
                _logger.LogCritical($"Something happened while deleting a report with id {id}");
                return StatusCode(500);
            }

            return RedirectToAction(nameof(All), "Report");
        }
    }
}
