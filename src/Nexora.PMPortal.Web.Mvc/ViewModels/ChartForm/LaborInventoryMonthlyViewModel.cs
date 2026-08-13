using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class LaborInventoryMonthlyViewModel
    {
        public string Month { get; set; }
        public decimal CentralOffice { get; set; }
        public decimal Project { get; set; }
        public decimal Total { get; set; }

    }
}
