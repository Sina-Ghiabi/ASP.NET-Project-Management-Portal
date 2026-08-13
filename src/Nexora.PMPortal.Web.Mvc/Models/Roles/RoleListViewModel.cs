using System.Collections.Generic;
using Nexora.PMPortal.Roles.Dto;

namespace Nexora.PMPortal.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<RoleListDto> Roles { get; set; }

        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
