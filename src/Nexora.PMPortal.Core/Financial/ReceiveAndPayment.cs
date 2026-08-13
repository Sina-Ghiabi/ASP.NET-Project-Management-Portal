using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial
{
    public class ReceiveAndPayment : FullAuditedEntity<int>
    {
        public int ProjectId { get; set; }
        public DateTime Year { get; set; }

        public decimal PreReceived { get; set; }
        public decimal Statements { get; set; }
        public decimal DepositRelease { get; set; }
        public decimal WarrantyReduce { get; set; }
        public decimal OtherReceipt { get; set; }
        public decimal IndirectReceipt { get; set; }
        public decimal OnAccountReceipt { get; set; }
        public decimal ProjectPayments { get; set; }
        public decimal WarrantyWithWage { get; set; }
        public decimal SaleryWithTax { get; set; }
        public decimal IndirecetPayment { get; set; }
        public decimal ValueAddedTax { get; set; }

        public virtual Project Project { get; set; }
    }
}
