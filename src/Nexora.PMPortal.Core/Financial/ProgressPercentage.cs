using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial
{
    public class ProgressPercentage : FullAuditedEntity<int>
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

        public decimal? TotalExecutionProgressPlan { get; set; }
        public decimal? TotalExecutionProgressReal { get; set; }

        public decimal TotalProgressPlan { get; set; }
        public decimal TotalProgressReal { get; set; }

        public virtual Project Project { get; set; }
    }
}
