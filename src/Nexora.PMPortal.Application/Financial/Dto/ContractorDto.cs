using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial.Dto
{
    [AutoMap(typeof(Contractor))]
    public class ContractorDto : FullAuditedEntityDto<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
