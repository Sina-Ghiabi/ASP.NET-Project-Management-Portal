using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectType
    {
        [Display(Name = "E")]
        E = 1,
        [Display(Name = "P")]
        P = 2,
        [Display(Name = "C")]
        C = 3,
        [Display(Name = "EP")]
        EP = 4,
        [Display(Name = "PC")]
        PC = 5,
        [Display(Name = "EPC")]
        EPC = 6,
        [Display(Name = "EPCF")]
        EPCF = 7,
        [Display(Name = "DB")]
        DB = 8,
        [Display(Name = "BOO")]
        BOO = 9,
        [Display(Name ="BOT")]
        BOT = 10
    }
}
