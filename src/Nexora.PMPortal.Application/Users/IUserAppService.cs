using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nexora.PMPortal.Roles.Dto;
using Nexora.PMPortal.Users.Dto;

namespace Nexora.PMPortal.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
