using System.ComponentModel.DataAnnotations;
using static BlogApp.Infrastructure.Common.ValidationConstants;

namespace BlogApp.Core.Models
{
    public class PostReportViewModel
    {
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        [Required(ErrorMessage = RequiredError)]
        [StringLength(ReportContentMax, MinimumLength = ReportContentMin, ErrorMessage = InputError)]
        public string ReportContent { get; set; }
    }
}
