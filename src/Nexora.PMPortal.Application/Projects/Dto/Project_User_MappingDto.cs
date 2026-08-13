using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects.Dto
{
    [AutoMap(typeof(Project_User_Mapping))]
    public class Project_User_MappingDto : FullAuditedEntityDto<int>
    {
        public int ProjectId { get; set; }
        public long UserId { get; set; }
    }
}
