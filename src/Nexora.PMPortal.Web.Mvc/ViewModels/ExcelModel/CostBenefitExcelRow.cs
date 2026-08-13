using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ExcelModel
{
    public class CostBenefitExcelRow
    {
        public int ProjectId { get; set; }
        public DateTime Year { get; set; }
        public int CostBenefitPeriod { get; set; }
        public int ServiceType { get; set; }
        public decimal ApprovedSales { get; set; }
        public decimal RecycleSales { get; set; }
        public decimal AllCost { get; set; }
        public decimal InflationRate { get; set; }
        public decimal GrossProfit { get; set; }
    }
}
