using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects.Dto
{
    [AutoMap(typeof(Project))]
    public class ProjectDto : FullAuditedEntityDto<int>
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
        public bool IsActive { get; set; }
        public int? UsageReportDisplayOrder { get; set; }
        public bool ShowInUsageReport { get; set; }
        public string AssetFileUrl { get; set; }
        public DateTime? AssetFileLastModifyDate { get; set; }
    }
}
