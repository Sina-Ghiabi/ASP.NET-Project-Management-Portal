using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ChartForm
{
    public class WarrantyBalanceViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int Year { get; set; }
        public string ObligationExecution { get; set; }
        public string PreReceived { get; set; }
        public string GuaranteeDeduction { get; set; }

        public int? LastGridPage { get; set; }

    }
}
