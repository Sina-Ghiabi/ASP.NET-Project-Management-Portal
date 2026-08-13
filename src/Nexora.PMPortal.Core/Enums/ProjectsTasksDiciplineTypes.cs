using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectsTasksDiciplineTypes
    {
        [Display(Name = "Civil & Architectural")]
        CivilAndArchitectural = 1,
        [Display(Name = "Electrical")]
        Electrical = 2,
        [Display(Name = "Instrument & Control")]
        InstrumentAndControl = 3,
        [Display(Name = "Mechanical")]
        Mechanical = 4,
        [Display(Name = "Piping")]
        Piping = 5,
        [Display(Name = "Process")]
        Process = 6,
        [Display(Name = "Quality Control")]
        QualityControl = 7,
        [Display(Name = "HSE")]
        HSE = 8,
        [Display(Name = "Safety")]
        Safety = 9,
        [Display(Name = "Project Control")]
        ProjectControl = 10
    }
}
