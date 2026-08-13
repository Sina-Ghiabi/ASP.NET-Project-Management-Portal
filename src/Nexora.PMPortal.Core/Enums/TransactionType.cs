using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Allocation")]
        Receive = 1,
        [Display(Name = "Payment")]
        Payment = 2,
    }
}
