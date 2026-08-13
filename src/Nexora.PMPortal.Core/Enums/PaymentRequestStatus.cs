using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum PaymentRequestStatus
    {
        [Display(Name = "Project Manager Approval Queue")]
        Waiting = 1,
        [Display(Name = "Planning Approval Queue")]
        CompanyWaiting = 2,
        [Display(Name = "Approved for Payment")]
        Confirmed = 3,
        [Display(Name = "Rejected")]
        Ignored = 10
    }
}
