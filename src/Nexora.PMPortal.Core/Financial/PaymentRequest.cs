using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial
{
    public class PaymentRequest : FullAuditedEntity<int>
    {
        public string ScheduleRow { get; set; }
        public string FourthLevelCode { get; set; }
        public string PaymentRequestCode { get; set; }
        public string PaymentDescription { get; set; }
        public PaymentRequestType? PaymentRequestType { get; set; }
        public string PayTo { get; set; }
        public decimal Amount { get; set; }
        public decimal? CreditBalance { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime? ChequeDueDate { get; set; }
        public string Description { get; set; }
        public int? ContractorId { get; set; }
        public PaymentRequestStatus PaymentRequestStatus { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual ICollection<Transaction> Payments { get; set; }

    }
}
