using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class IndexViewModel
    {
        public List<ProjectViewModel> Projects { get; set; }
        public ReportYear ReportYear { get; set; }

        public int MyProjectCount { get; set; }

    }

}
