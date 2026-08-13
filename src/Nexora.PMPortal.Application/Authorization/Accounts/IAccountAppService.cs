using System.Threading.Tasks;
using Abp.Application.Services;
using Nexora.PMPortal.Authorization.Accounts.Dto;

namespace Nexora.PMPortal.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
