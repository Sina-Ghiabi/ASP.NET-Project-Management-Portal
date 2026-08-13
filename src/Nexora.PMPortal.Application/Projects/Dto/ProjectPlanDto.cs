using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects.Dto
{
    [AutoMap(typeof(ProjectPlan))]
    public class ProjectPlanDto : FullAuditedEntityDto<int>
    {
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public ProjectPlanType ProjectPlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public bool IsActive { get; set; }
    }

}
