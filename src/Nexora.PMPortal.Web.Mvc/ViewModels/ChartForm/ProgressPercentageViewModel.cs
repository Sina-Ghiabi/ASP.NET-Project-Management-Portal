using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class ProgressPercentageViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public SelectiveYear Year { get; set; }
        public SelectiveMonth Month { get; set; }

        public string EngineeringProgressPlan { get; set; }
        public string EngineeringProgressReal { get; set; }

        public string SupplyProgressPlan { get; set; }
        public string SupplyProgressReal { get; set; }

        public string ExecutionProgressPlan { get; set; }
        public string ExecutionProgressReal { get; set; }

        public string TotalExecutionProgressPlan { get; set; }
        public string TotalExecutionProgressReal { get; set; }

        public string TotalProgressPlan { get; set; }
        public string TotalProgressReal { get; set; }

        public int? LastGridPage { get; set; }
    }
}
