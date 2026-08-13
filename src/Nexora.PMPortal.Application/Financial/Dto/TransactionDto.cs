using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial.Dto
{

    [AutoMap(typeof(Transaction))]
    public class TransactionDto : FullAuditedEntityDto<int>
    {
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime? ChequeDueDate { get; set; }
        public string Description { get; set; }


        //Payment request payments
        public int? PaymentRequestId { get; set; }

        //Project allocations
        public int? ReceiveBillId { get; set; }
        public int? ProjectId { get; set; }
    }
}
