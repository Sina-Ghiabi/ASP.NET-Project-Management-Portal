using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Managers
{
    public class ManagerReport : FullAuditedEntity<int>, IPassivable
    {
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ManagerReportType ManagerReportType { get; set; }
        public bool IsActive { get; set; }
    }
}
