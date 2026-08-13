using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial
{
    public class Katibe_Darkhast : AuditedEntity<int>
    {
        public string DarVajh { get; set; }
        public string Kharid { get; set; }
        public string Onvan { get; set; }
        public string CodeOnvan { get; set; }
        public int? ShenaseOnvan { get; set; }
        public string TaaedMali { get; set; }
        public decimal? Shenasname { get; set; }

    }
}
