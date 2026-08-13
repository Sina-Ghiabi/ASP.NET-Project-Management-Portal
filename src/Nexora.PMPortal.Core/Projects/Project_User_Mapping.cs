using Abp.Domain.Entities.Auditing;
using Nexora.PMPortal.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Projects
{
    public class Project_User_Mapping : FullAuditedEntity<int>
    {
        public int ProjectId { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}
