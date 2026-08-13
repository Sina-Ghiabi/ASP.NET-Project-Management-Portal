using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial.Dto
{
    [AutoMap(typeof(WarrantyBalance))]
    public class WarrantyBalanceDto : FullAuditedEntityDto<int>
    {
        public int ProjectId { get; set; }
        public DateTime Year { get; set; }
        public decimal ObligationExecution { get; set; }
        public decimal PreReceived { get; set; }
        public decimal GuaranteeDeduction { get; set; }
        public virtual Project Project { get; set; }
    }
}
