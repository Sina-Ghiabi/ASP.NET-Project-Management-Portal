using System.Collections.Generic;
using Nexora.PMPortal.Roles.Dto;
using Nexora.PMPortal.Users.Dto;

namespace Nexora.PMPortal.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
