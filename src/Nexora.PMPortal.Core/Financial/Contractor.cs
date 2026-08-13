using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial
{
    public class Contractor : FullAuditedEntity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PaymentRequest> PaymentRequests { get; set; }
    }
}