using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects
{
    public class ProjectReportFile : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public DateTime? SubmitDate { get; set; }

        public int ProjectReportId { get; set; }
        public virtual ProjectReport ProjectReport { get; set; }

    }
}
