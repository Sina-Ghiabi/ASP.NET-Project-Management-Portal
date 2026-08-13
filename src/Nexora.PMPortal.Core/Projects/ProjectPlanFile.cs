using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects
{
    public class ProjectPlanFile : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public DateTime? SubmitDate { get; set; }

        public int ProjectPlanId { get; set; }
        public virtual ProjectPlan ProjectPlan { get; set; }

    }
}
