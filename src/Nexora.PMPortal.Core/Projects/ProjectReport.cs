using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects
{
    public class ProjectReport : FullAuditedEntity<int>, IPassivable
    {
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public ProjectReportType ProjectReportType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ProjectReportFile> ProjectReportFiles { get; set; }

    }

}