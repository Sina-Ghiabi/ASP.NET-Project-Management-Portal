using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels.ExcelModel
{
    public class ExcelRecieptViewModel
    {
        public int Key { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal Amount { get; set; }
        public int PaymentType { get; set; }
        public DateTime? ChequeDueDate { get; set; }
        public int CurrencyType { get; set; }
    }

    public class ExcelRecieptTransactionViewModel
    {
        public int ParentKey { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyType { get; set; }
    }

}
