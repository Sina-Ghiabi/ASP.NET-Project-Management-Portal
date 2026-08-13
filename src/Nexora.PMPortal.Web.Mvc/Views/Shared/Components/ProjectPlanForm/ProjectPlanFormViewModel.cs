using System.Collections.Generic;
using Abp.Localization;
using Nexora.PMPortal.Enums;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ProjectPlanForm
{
    public class ProjectPlanFormViewModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public ProjectPlanType ProjectPlanType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ProjectId { get; set; }
    }

}
