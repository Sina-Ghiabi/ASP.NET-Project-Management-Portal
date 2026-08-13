using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial
{
    public class Transaction : FullAuditedEntity<int>
    {
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime? ChequeDueDate { get; set; }
        public string Description { get; set; }


        //Payment request payments
        public int? PaymentRequestId { get; set; }
        public virtual PaymentRequest PaymentRequest { get; set; }


        //Project allocations
        public int? ReceiveBillId { get; set; }
        public virtual ReceiveBill ReceiveBill { get; set; }
        public int? ProjectId { get; set; }
        public virtual Project Project { get; set; }

    }
}
