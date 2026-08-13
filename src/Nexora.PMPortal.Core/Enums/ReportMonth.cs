using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ReportMonth
    {
        [Display(Name = "All Months")]
        M_0 = 0,
        [Display(Name = "Farvardin")]
        M_1 = 1,
        [Display(Name = "Ordibehesht")]
        M_2 = 2,
        [Display(Name = "Khordad")]
        M_3 = 3,
        [Display(Name = "Tir")]
        M_4 = 4,
        [Display(Name = "Mordad")]
        M_5 = 5,
        [Display(Name = "Shahrivar")]
        M_6 = 6,
        [Display(Name = "Mehr")]
        M_7 = 7,
        [Display(Name = "Aban")]
        M_8 = 8,
        [Display(Name = "Azar")]
        M_9 = 9,
        [Display(Name = "Dey")]
        M_10 = 10,
        [Display(Name = "Bahman")]
        M_11 = 11,
        [Display(Name = "Esfand")]
        M_12 = 12,
    }
}
