using Abp.Authorization;
using Nexora.PMPortal.Authorization.Roles;
using Nexora.PMPortal.Authorization.Users;

namespace Nexora.PMPortal.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
