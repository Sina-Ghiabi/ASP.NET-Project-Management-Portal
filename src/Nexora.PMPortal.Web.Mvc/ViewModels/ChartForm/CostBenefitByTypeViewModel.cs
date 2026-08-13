using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class CostBenefitByTypeViewModel
    {
        public int Year { get; set; }
        public string LastMonth { get; set; }
        public decimal Total { get; set; }
        public decimal WaterAndWasteWater { get; set; }
        public decimal ElectricityAndEnergy { get; set; }
        public decimal Utilty { get; set; }
        public decimal WaterSupply { get; set; }
        public decimal IndustrialDevelopment { get; set; }

    }
}
