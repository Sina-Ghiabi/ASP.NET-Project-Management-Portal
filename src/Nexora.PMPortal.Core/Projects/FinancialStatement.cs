using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects
{
    public class FinancialStatement : FullAuditedEntity<int>
    {
        public FinancialStatementType FinancialStatementType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string Number { get; set; }
        public string Period { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PeriodicAmount { get; set; }
        public string FileUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

    }
}
