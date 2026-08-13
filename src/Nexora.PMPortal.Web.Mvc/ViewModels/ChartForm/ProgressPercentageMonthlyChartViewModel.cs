using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class ProgressPercentageMonthlyChartViewModel
    {
        public int Index { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public decimal TotalPlan { get; set; }
        public decimal TotalReal { get; set; }
    }
}
