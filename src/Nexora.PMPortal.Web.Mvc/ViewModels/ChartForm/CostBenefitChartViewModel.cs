using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class CostBenefitChartViewModel
    {
        public int Year { get; set; }
        public string LastMonth { get; set; }
        public decimal Amount { get; set; }
    }
}
