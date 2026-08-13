using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class ResourceAndUsageViewModel
    {
        public List<ProjectViewModel> Projects { get; set; }
        public List<ResourceAndUsageRowViewModel> Rows { get; set; }
        public List<ReceiveBillsRowViewModel> ReceiveBills { get; set; }

        public List<PaymentsRowViewModel> Payments { get; set; }

        public List<CashRemainingRowViewModel> CashReaminings { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public ReportYear ReportYear { get; set; }

        public decimal TotalSave { get; set; }

    }


    public class ResourceAndUsageRowViewModel
    {
        public DateTime DateTime { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<ResourceAndUsageColumnViewModel> ProjectUsages { get; set; }
        public ResourceAndUsageRowViewModel()
        {
            this.ProjectUsages = new List<ResourceAndUsageColumnViewModel>();
        }
    }

    public class ResourceAndUsageColumnViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal TodayTotalAmount { get; set; }
        public bool ShowInTable { get; set; }
        public int? DisplayOrder { get; set; }
    }



    public class ReceiveBillsRowViewModel
    {
        public int Year { get; set; }
        public List<ReceiveBillsColumnViewModel> ProjectBills { get; set; }
        public ReceiveBillsRowViewModel()
        {
            this.ProjectBills = new List<ReceiveBillsColumnViewModel>();
        }
    }

    public class PaymentsRowViewModel
    {
        public int Year { get; set; }
        public List<PaymentsColumnViewModel> ProjectPayments { get; set; }
        public PaymentsRowViewModel()
        {
            this.ProjectPayments = new List<PaymentsColumnViewModel>();
        }
    }


    public class ReceiveBillsColumnViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal YearTotalAmount { get; set; }
        public bool ShowInTable { get; set; }
        public int? DisplayOrder { get; set; }
    }


    public class PaymentsColumnViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal YearTotalAmount { get; set; }
        public bool ShowInTable { get; set; }
        public int? DisplayOrder { get; set; }
    }



    public class CashRemainingRowViewModel
    {
        public int Year { get; set; }
        public List<CashRemainingColumnViewModel> ProjectCashRemainings { get; set; }
        public CashRemainingRowViewModel()
        {
            this.ProjectCashRemainings = new List<CashRemainingColumnViewModel>();
        }
    }
    public class CashRemainingColumnViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public decimal YearTotalAmount { get; set; }
        public bool ShowInTable { get; set; }
        public int? DisplayOrder { get; set; }
    }

}
