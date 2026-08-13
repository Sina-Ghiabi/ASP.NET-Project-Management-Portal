using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ExcelModel
{
    public class WarrantyBalanceExcelRow
    {
        public int ProjectId { get; set; }
        public DateTime Year { get; set; }
        public decimal ObligationExecution { get; set; }
        public decimal PreReceived { get; set; }
        public decimal GuaranteeDeduction { get; set; }
    }
}
