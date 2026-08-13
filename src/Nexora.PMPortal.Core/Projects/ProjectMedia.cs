using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects
{
    public class ProjectMedia : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? SubmitDate { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

    }
}
