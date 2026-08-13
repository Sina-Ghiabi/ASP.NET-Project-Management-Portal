using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ReceiveBillType
    {
        [Display(Name = "Cash")]
        Cash = 1,
        [Display(Name = "Cheque")]
        Cheque = 2,
    }
}
