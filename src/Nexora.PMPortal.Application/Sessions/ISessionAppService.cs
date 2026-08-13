using System.Threading.Tasks;
using Abp.Application.Services;
using Nexora.PMPortal.Sessions.Dto;

namespace Nexora.PMPortal.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
