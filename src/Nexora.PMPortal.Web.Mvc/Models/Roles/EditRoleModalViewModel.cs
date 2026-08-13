using Abp.AutoMapper;
using Nexora.PMPortal.Roles.Dto;
using Nexora.PMPortal.Web.Models.Common;

namespace Nexora.PMPortal.Web.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class EditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool HasPermission(PermissionDto permission)
        {
            return GrantedPermissionNames.Contains(permission.Name);
        }
    }
}
