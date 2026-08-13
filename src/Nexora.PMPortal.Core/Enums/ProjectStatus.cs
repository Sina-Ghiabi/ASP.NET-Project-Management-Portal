using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectStatus
    {
        [Display(Name = "Advancement Phase")]
        AdvancementEra = 1,
        [Display(Name = "In Progress")]
        InProcess = 2,
        [Display(Name = "Temporary Handover")]
        TemporaryDelivery = 3,
        [Display(Name = "In Operation")]
        InProduction = 4,
        [Display(Name = "Final Handover")]
        FnalDelivery = 5,
        [Display(Name = "Suspended")]
        Suspended = 6,
        [Display(Name = "Closure Phase")]
        Closure = 7,
        [Display(Name = "Transferred")]
        Transferred = 8

    }
}
