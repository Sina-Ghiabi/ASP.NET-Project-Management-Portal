using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum CurrencyType
    {
        [Display(Name = "Iranian Rial")]
        IRR = 1,
        [Display(Name = "Euro")]
        EUR = 2,
        [Display(Name = "UAE Dirham")]
        AED = 3,
        [Display(Name = "Chinese Yuan")]
        CNY = 4,
        [Display(Name = "US Dollar")]
        USD = 5,
    }
}
