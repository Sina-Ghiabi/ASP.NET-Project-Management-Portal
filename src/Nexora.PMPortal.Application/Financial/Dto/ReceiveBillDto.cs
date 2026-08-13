using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial.Dto
{

    [AutoMap(typeof(ReceiveBill))]
    public class ReceiveBillDto : FullAuditedEntityDto<int>
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
    }
}
