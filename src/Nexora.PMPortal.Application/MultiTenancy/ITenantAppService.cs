using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nexora.PMPortal.MultiTenancy.Dto;

namespace Nexora.PMPortal.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

