using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects.Dto
{
    [AutoMap(typeof(ProjectMedia))]
    public class ProjectMediaDto : FullAuditedEntityDto<int>
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? SubmitDate { get; set; }
        public int ProjectId { get; set; }
    }
}
