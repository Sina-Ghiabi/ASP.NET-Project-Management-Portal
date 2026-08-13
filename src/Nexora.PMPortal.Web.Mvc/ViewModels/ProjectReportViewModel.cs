using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class ProjectReportViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public ProjectReportType ProjectReportType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ProjectId { get; set; }
    }
}
