using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectsTasksStatusTypes
    {
        [Display(Name = "AFC")]
        AFC = 1,
        [Display(Name = "IFA")]
        IFA = 2,
        [Display(Name = "IFC")]
        IFC = 3,
        [Display(Name = "IFI")]
        IFI = 4,
        [Display(Name = "IFR")]
        IFR = 5,
        [Display(Name = "AFD")]
        AFD = 6,
        [Display(Name = "IFB")]
        IFB = 7
    }
}
