using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class CostBenefitViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public int Year { get; set; }
        public CostBenefitPeriod CostBenefitPeriod { get; set; }
        public CostBenefitServiceType ServiceType { get; set; }

        public string ApprovedSales { get; set; }
        public string RecycleSales { get; set; }
        public string AllCost { get; set; }
        public string GrossProfit { get; set; }
        public string Profit { get; set; }
        public string InflationRate { get; set; }

        public int? LastGridPage { get; set; }
    }
}
