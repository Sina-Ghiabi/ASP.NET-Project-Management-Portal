using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial
{
    public class GrossProfitToNet : FullAuditedEntity<int>
    {
        public DateTime Year { get; set; }
        public decimal NetProfit { get; set; }
        public decimal GrossProfit { get; set; }

        public decimal SaleCost { get; set; }
        public decimal OperationalProfit { get; set; }
        public decimal FinancialCost { get; set; }
        public decimal NonOperationalCost { get; set; }
        public decimal OperationalCost { get; set; }
        public decimal IncomeTax { get; set; }

    }
}
