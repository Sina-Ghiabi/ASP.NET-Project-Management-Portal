using System.Collections.Generic;
using Abp.Localization;
using Nexora.PMPortal.Enums;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ManagerReportForm
{
    public class ManagerReportFormViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

}
