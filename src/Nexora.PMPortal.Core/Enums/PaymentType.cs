using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum PaymentType
    {
        [Display(Name = "Cash")]
        Cash = 1,
        [Display(Name = "Post-dated Cheque")]
        Cheque = 2,
        [Display(Name = "Same-day Cheque")]
        SameDayCheque = 3,
    }
}
