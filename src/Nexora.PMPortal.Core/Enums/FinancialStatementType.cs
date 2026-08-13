using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum FinancialStatementType
    {
        [Display(Name = "Temporary Statement")]
        Temporary = 1,
        [Display(Name = "Final Statement")]
        Final = 2,
        [Display(Name = "Adjustment")]
        Adjustment = 3,
    }
}
