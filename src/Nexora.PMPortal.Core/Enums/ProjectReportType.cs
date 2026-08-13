using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectReportType
    {
        [Display(Name = "Daily")]
        Daily = 0,
        [Display(Name = "CEO Weekly")]
        Weekly = 1,
        [Display(Name = "Management Weekly")]
        ManagaementWeekly = 2,
        [Display(Name = "HR Daily")]
        HrDaily = 3,
        [Display(Name = "Monthly")]
        Monthly = 4,
        [Display(Name = "Employer")]
        Contractor = 5,
        [Display(Name = "HSE")]
        HSE = 6,
        [Display(Name = "Other")]
        Other = 7

    }
}