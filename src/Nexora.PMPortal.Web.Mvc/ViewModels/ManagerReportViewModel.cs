using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class ManagerReportViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreationTime { get; set; }
        public ManagerReportType ManagerReportType { get; set; }
    }
}
