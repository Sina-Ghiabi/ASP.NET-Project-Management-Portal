using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.Models.Users
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string LastLoginTime { get; set; }
        public string CreationTime { get; set; }
        public string RoleNames { get; set; }
    }
}
