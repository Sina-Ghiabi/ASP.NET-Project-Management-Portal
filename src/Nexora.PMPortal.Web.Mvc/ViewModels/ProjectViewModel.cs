using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Projects.Dto;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public ProjectType ProjectType { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public CurrencyType? AmountCurrencyType { get; set; }
        public string Client { get; set; }
        public string PlannigManager { get; set; }
        public string Consultant { get; set; }
        public string ProjectManager { get; set; }
        public string Partners { get; set; }
        public string PartnerShares { get; set; }
        public string InitialContractAmount { get; set; }
        public string ExtensionContractAmount { get; set; }
        public string InitialContractAmountInCurrency { get; set; }
        public string ExtensionContractAmountInCurrency { get; set; }
        public string ProjectSite { get; set; }
        public string StartDate { get; set; }
        public string TemporaryDeliveryDate { get; set; }
        public string FinalDeliveryDate { get; set; }
        public string ProjectContractPeriod { get; set; }
        public string ProjectCompletionDate { get; set; }
        public string ContractEndDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? UsageReportDisplayOrder { get; set; }
        public bool ShowInUsageReport { get; set; }

        public string FriendlyCreationTime { get; set; }


        public string LastFinalFinancialStatemntValue { get; set; }
        public string LastTemporaryFinancialStatemntValue { get; set; }
        public string LastAdjustmentFinancialStatemntValue { get; set; }
        public string AssetFileUrl { get; set; }
        public string AssetFileLastModifyDate { get; set; }

        public FinancialStatementViewModel FinancialStatementForm { get; set; }
        public ProjectReportViewModel ProjectReportForm { get; set; }
        public ProjectPlanViewModel ProjectPlanForm { get; set; }
        public ProjectMediaViewModel ProjectMediaForm { get; set; }
        public List<ProjectMediaViewModel> ProjectMedias { get; set; }

        public ProjectViewModel(){}

        public ProjectViewModel(ProjectDto item)
        {
            Id = item.Id;
            Title = item.Title;
            Code = item.Code;
            ProjectType = item.ProjectType;
            ProjectStatus = item.ProjectStatus;
            Client = item.Client;
            PlannigManager = item.PlannigManager;
            Consultant = item.Consultant;
            ProjectManager = item.ProjectManager;
            Partners = item.Partners;
            PartnerShares = item.PartnerShares;
            AmountCurrencyType = item.AmountCurrencyType;
            ProjectSite = item.ProjectSite;
            InitialContractAmount = item.InitialContractAmount !=null ? item.InitialContractAmount.Value.ToString("N0") : null;
            ExtensionContractAmount = item.ExtensionContractAmount != null ? item.ExtensionContractAmount.Value.ToString("N0") : null;
            InitialContractAmountInCurrency = item.InitialContractAmountInCurrency != null ? item.InitialContractAmountInCurrency.Value.ToString("N0") : null;
            ExtensionContractAmountInCurrency = item.ExtensionContractAmountInCurrency != null ? item.ExtensionContractAmountInCurrency.Value.ToString("N0") : null;
            StartDate = item.StartDate != null ? new PersianDateTime(item.StartDate).ToShortDateString() : null;
            TemporaryDeliveryDate = item.TemporaryDeliveryDate != null ? new PersianDateTime(item.TemporaryDeliveryDate).ToShortDateString() : null;
            FinalDeliveryDate = item.FinalDeliveryDate != null ? new PersianDateTime(item.FinalDeliveryDate).ToShortDateString() : null;
            ProjectContractPeriod = item.ProjectContractPeriod;
            ProjectCompletionDate = item.ProjectCompletionDate != null ? new PersianDateTime(item.ProjectCompletionDate).ToShortDateString() : null;
            ContractEndDate = item.ContractEndDate != null ? new PersianDateTime(item.ContractEndDate).ToShortDateString() : null;
            Description = item.Description;
            UsageReportDisplayOrder = item.UsageReportDisplayOrder;
            ShowInUsageReport = item.ShowInUsageReport;
            AssetFileUrl = item.AssetFileUrl;
            AssetFileLastModifyDate = item.AssetFileLastModifyDate != null ? new PersianDateTime(item.AssetFileLastModifyDate).ToShortDateString() : null;

        }

    }
}
