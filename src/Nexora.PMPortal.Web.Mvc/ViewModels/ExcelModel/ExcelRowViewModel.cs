using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ExcelModel
{
    public class ExcelRowViewModel
    {
        public int ProjectId { get; set; }
        public DateTime CreationTime { get; set; }
        public string FourthLevelCode { get; set; }
        public string PaymentRequestCode { get; set; }
        public string Description { get; set; }
        public int PaymentRequestType { get; set; }
        public string PayTo { get; set; }
        public decimal Amount { get; set; }
        public int PaymentType { get; set; }
        public DateTime? ChequeDueDate { get; set; }
        public int CurrencyType { get; set; }
        public string MoreDescription { get; set; }
    }
}
