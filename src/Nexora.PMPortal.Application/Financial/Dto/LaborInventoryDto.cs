using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial.Dto
{
    [AutoMap(typeof(LaborInventory))]
    public class LaborInventoryDto : FullAuditedEntityDto<int>
    {
        public int ProjectId { get; set; }
        public DateTime Year { get; set; }
        public decimal Month1_Amount { get; set; }
        public decimal Month2_Amount { get; set; }
        public decimal Month3_Amount { get; set; }
        public decimal Month4_Amount { get; set; }
        public decimal Month5_Amount { get; set; }
        public decimal Month6_Amount { get; set; }
        public decimal Month7_Amount { get; set; }
        public decimal Month8_Amount { get; set; }
        public decimal Month9_Amount { get; set; }
        public decimal Month10_Amount { get; set; }
        public decimal Month11_Amount { get; set; }
        public decimal Month12_Amount { get; set; }

        public virtual Project Project { get; set; }
    }
}
