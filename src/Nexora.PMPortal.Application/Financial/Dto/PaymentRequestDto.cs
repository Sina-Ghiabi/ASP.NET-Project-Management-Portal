using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial.Dto
{

    [AutoMap(typeof(PaymentRequest))]
    public class PaymentRequestDto : FullAuditedEntityDto<int>
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
        public PaymentRequestStatus PaymentRequestStatus { get; set; }
        public int ProjectId { get; set; }
    }
}
