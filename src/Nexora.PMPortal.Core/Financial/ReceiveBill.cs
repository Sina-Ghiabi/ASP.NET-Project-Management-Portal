using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial
{
    public class ReceiveBill : FullAuditedEntity<int>
    {
        public ReceiveBillType ReceiveBillType { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime? ChequeDueDate { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string Description { get; set; }
        public string ReceiveDescription { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Transaction> Assignemnts { get; set; }

    }
}
