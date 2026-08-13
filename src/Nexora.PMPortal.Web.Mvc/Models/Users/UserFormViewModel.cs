using Abp.Auditing;
using Abp.Authorization.Users;
using Nexora.PMPortal.Roles.Dto;
using Nexora.PMPortal.Users.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.Models.Users
{
    public class UserFormViewModel
    {
        public long Id { get; set; }
        public CreateUserDto User { get; set; }
        public string[] SelectedRoles { get; set; }
        public IReadOnlyList<RoleDto> Roles { get; set; }
        public string NewPassword { get; set; }
    }
}
