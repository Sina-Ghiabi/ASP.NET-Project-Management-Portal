using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Financial;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects
{
    public class Project : FullAuditedEntity<int>, IPassivable
    {
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
        public decimal? InitialContractAmount { get; set; }
        public decimal? ExtensionContractAmount { get; set; }
        public decimal? InitialContractAmountInCurrency { get; set; }
        public decimal? ExtensionContractAmountInCurrency { get; set; }
        public string ProjectSite { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TemporaryDeliveryDate { get; set; }
        public DateTime? FinalDeliveryDate { get; set; }
        public string ProjectContractPeriod { get; set; }
        public DateTime? ProjectCompletionDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string Description { get; set; }
        public int? UsageReportDisplayOrder { get; set; }
        public bool ShowInUsageReport { get; set; }
        public bool IsActive { get; set; }
        public string AssetFileUrl { get; set; }
        public DateTime? AssetFileLastModifyDate { get; set; }

        public virtual ICollection<ProjectMedia> ProjectMedias { get; set; }
        public virtual ICollection<FinancialStatement> FinancialStatements { get; set; }
        public virtual ICollection<ProjectReport> ProjectReports { get; set; }
        public virtual ICollection<Transaction> ProjectAssignedBudget{ get; set; }
        public virtual ICollection<Project_User_Mapping> Project_User_Mappings { get; set; }

        public Project()
        {
            CreationTime = DateTime.Now;
        }
    }
}
