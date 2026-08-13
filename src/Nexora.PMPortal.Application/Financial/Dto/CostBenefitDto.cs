using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial.Dto
{
    [AutoMap(typeof(CostBenefit))]
    public class CostBenefitDto : FullAuditedEntityDto<int>
    {
        public int ProjectId { get; set; }
        public DateTime Year { get; set; }
        public CostBenefitPeriod CostBenefitPeriod { get; set; }
        public CostBenefitServiceType ServiceType { get; set; }
        public decimal ApprovedSales { get; set; }
        public decimal RecycleSales { get; set; }
        public decimal AllCost { get; set; }
        public decimal InflationRate { get; set; }
        public decimal GrossProfit { get; set; }
        public virtual Project Project { get; set; }
    }
}
