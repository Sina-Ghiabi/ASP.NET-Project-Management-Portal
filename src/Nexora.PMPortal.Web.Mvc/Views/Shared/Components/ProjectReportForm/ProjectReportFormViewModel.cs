using System.Collections.Generic;
using Abp.Localization;
using Nexora.PMPortal.Enums;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ProjectReportForm
{
    public class ProjectReportFormViewModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public ProjectReportType ProjectReportType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ProjectId { get; set; }
    }

}
