using System.Collections.Generic;
using Nexora.PMPortal.Roles.Dto;

namespace Nexora.PMPortal.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<PermissionDto> Permissions { get; set; }
    }
}