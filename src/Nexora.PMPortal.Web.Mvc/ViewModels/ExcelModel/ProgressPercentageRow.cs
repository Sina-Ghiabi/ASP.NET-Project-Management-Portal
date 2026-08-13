using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ExcelModel
{
    public class ProgressPercentageRow
    {
        public int ProjectId { get; set; }
        public SelectiveYear Year { get; set; }
        public SelectiveMonth Month { get; set; }

        public decimal EngineeringProgressPlan { get; set; }
        public decimal EngineeringProgressReal { get; set; }

        public decimal SupplyProgressPlan { get; set; }
        public decimal SupplyProgressReal { get; set; }

        public decimal ExecutionProgressPlan { get; set; }
        public decimal ExecutionProgressReal { get; set; }

        public decimal TotalExecutionProgressPlan { get; set; }
        public decimal TotalExecutionProgressReal { get; set; }

        public decimal TotalProgressPlan { get; set; }
        public decimal TotalProgressReal { get; set; }
    }
}
