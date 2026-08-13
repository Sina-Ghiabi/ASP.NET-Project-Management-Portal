using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nexora.PMPortal.Enums;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class CostBenefitSaleChartViewModel
    {
        public int Index { get; set; }
        public int Year { get; set; }
        public string LastMonth { get; set; }
        public decimal YearSale { get; set; }
        public decimal TotalSales { get; set; }
        public decimal YearProfit { get; set; }
        public decimal TotalProfits { get; set; }
        public decimal GrossProfit { get; set; }

    }
}
