using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Managers.Dto
{
    [AutoMap(typeof(ManagerReport))]
    public class ManagerReportDto : FullAuditedEntityDto<int>
    {
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public ManagerReportType ManagerReportType { get; set; }
    }

}
