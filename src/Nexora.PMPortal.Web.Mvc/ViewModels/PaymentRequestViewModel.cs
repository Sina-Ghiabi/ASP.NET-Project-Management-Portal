using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Financial.Dto;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class PaymentRequestViewModel
    {
        public int Id { get; set; }
        public string ScheduleRow { get; set; }
        public string FourthLevelCode { get; set; }
        public string PaymentRequestCode { get; set; }
        public string PaymentDescription { get; set; }
        public PaymentRequestType? PaymentRequestType { get; set; }
        public string PayTo { get; set; }
        public string Amount { get; set; }
        public string CreditBalance { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public PaymentType PaymentType { get; set; }
        public string ChequeDueDate { get; set; }
        public string Description { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal PaidAmount { get; set; }
        public string CreationDate { get; set; }

        public int PaymentsCount { get; set; }
        public int? PaymentRequestStatus { get; set; }

        public int? LastGridPage { get; set; }
        public string RefererPage { get; set; }

        public PaymentRequestViewModel() { }

        public PaymentRequestViewModel(PaymentRequestDto item)
        {
            Id = item.Id;
            ScheduleRow = item.ScheduleRow;
            FourthLevelCode = item.FourthLevelCode;
            PaymentRequestCode = item.PaymentRequestCode;
            PaymentDescription = item.PaymentDescription;
            PaymentRequestType = item.PaymentRequestType;
            PayTo = item.PayTo;
            Amount = item.Amount.ToString("0.");
            CreditBalance = item.CreditBalance != null ? item.CreditBalance.Value.ToString("0.") : null;
            CurrencyType = item.CurrencyType;
            PaymentType = item.PaymentType;
            Description = item.Description;
            ProjectId = item.ProjectId;
            ChequeDueDate = item.ChequeDueDate != null ? new PersianDateTime(item.ChequeDueDate).ToShortDateString() : null;
        }

    }


}
