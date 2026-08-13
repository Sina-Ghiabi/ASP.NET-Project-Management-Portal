using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ReportYear
    {
        [Display(Name = "All Years")]
        All = 0,
        [Display(Name = "1397")]
        Y_1397 = 1397,
        [Display(Name = "1398")]
        Y_1398 = 1398,
        [Display(Name = "1399")]
        Y_1399 = 1399,
        [Display(Name = "1400")]
        Y_1400 = 1400,
        [Display(Name = "1401")]
        Y_1401 = 1401,
    }
}
