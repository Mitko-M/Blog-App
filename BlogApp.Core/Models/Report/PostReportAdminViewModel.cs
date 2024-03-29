using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models.Report
{
    public class PostReportAdminViewModel : PostReportViewModel
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string ReporterUserName { get; set; }
    }
}
