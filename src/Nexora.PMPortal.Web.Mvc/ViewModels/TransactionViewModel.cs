using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string CreationTime { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }

        public int? RefrenceId { get; set; }
        public PaymentRequestType? PaymentRequest_Type { get; set; }
        public string PaymentRequest_PayTo { get; set; }
        public string PaymentRequest_FourthLevelCode { get; set; }
        public string PaymentRequest_ChequeDueDate { get; set; }
        public string PaymentRequest_PaymentRequestCode { get; set; }
        public string PaymentRequest_PaymentDescription { get; set; }
        public string PaymentRequest_Description { get; set; }

        public string RefrenceDescription { get; set; }

        public TransactionViewModel()
        {

        }
    }
}
