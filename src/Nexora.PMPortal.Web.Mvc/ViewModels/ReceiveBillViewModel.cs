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
    public class ReceiveBillViewModel
    {
        public int Id { get; set; }
        public ReceiveBillType ReceiveBillType { get; set; }
        public string Amount { get; set; }
        public decimal AssignedAmount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public PaymentType PaymentType { get; set; }
        public string ChequeDueDate { get; set; }
        public string ReceiveDate { get; set; }
        public string Description { get; set; }
        public string ReceiveDescription { get; set; }
        public string CreationDate { get; set; }
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
        public decimal Remaining { get; set; }

        public ReceiveBillViewModel() { }

        public ReceiveBillViewModel(ReceiveBillDto item)
        {
            Id = item.Id;
            ReceiveBillType = item.ReceiveBillType;
            Amount = item.Amount.ToString("0.");
            CurrencyType = item.CurrencyType;
            PaymentType = item.PaymentType;
            CurrencyType = item.CurrencyType;
            PaymentType = item.PaymentType;
            Description = item.Description;
            ReceiveDescription = item.ReceiveDescription;
            ProjectId = item.ProjectId;
            ChequeDueDate = item.ChequeDueDate != null ? new PersianDateTime(item.ChequeDueDate).ToShortDateString() : null;
            ReceiveDate = item.ReceiveDate != null ? new PersianDateTime(item.ReceiveDate).ToShortDateString() : null;
        }



    }
}
