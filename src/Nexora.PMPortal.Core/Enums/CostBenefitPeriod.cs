using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum CostBenefitPeriod
    {
        [Display(Name = "3 Months")]
        Three_Month = 1,
        [Display(Name = "6 Months")]
        Six_Month = 2,
        [Display(Name = "9 Months")]
        Nine_Month = 3,
        [Display(Name = "Annual")]
        Twelve_Month = 4,
    }
}
