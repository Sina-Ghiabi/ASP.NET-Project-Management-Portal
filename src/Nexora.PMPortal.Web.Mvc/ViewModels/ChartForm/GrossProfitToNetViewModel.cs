using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class GrossProfitToNetViewModel
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string NetProfit { get; set; }
        public string GrossProfit { get; set; }
        public string SaleCost { get; set; }
        public string OperationalProfit { get; set; }
        public string FinancialCost { get; set; }
        public string OperationalCost { get; set; }
        public string NonOperationalCost { get; set; }
        public string IncomeTax { get; set; }

        public int? LastGridPage { get; set; }
    }
}
