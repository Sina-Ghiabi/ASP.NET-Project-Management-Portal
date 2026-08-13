using Abp.AutoMapper;
using Nexora.PMPortal.Sessions.Dto;

namespace Nexora.PMPortal.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}
