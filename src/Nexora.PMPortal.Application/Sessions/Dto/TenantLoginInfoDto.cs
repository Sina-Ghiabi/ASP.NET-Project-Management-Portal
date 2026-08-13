using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Nexora.PMPortal.MultiTenancy;

namespace Nexora.PMPortal.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
