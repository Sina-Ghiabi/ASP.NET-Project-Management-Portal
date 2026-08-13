using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nexora.PMPortal.Financial.Dto
{
    [AutoMap(typeof(Katibe_Darkhast))]
    public class Katibe_DarkhastDto : AuditedEntityDto<int>
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
