using System.Collections.Generic;
using System.Linq;
using Nexora.PMPortal.Roles.Dto;
using Nexora.PMPortal.Users.Dto;

namespace Nexora.PMPortal.Web.Models.Users
{
    public class EditUserModalViewModel
    {
        public UserDto User { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }

        public bool UserIsInRole(RoleDto role)
        {
            return User.RoleNames != null && User.RoleNames.Any(r => r == role.NormalizedName);
        }
    }
}
