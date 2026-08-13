using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectPlanType
    {
        [Display(Name = "Monthly Budget Plan")]
        MonthlyBudgetPlan = 0,
        [Display(Name = "Schedule Plan")]
        TimingPlan = 1,
        [Display(Name = "Other")]
        Other = 2
    }
}