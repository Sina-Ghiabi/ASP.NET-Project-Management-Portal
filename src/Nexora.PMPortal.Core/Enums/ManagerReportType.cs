using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ManagerReportType
    {
        [Display(Name = "Weekly")]
        Weekly = 1,
        [Display(Name = "Monthly")]
        Monthly = 2,
        [Display(Name = "Board of Directors")]
        Board = 3,
        [Display(Name = "General Assembly")]
        Convention = 4,
        [Display(Name = "Human Resources")]
        HR = 5,
        [Display(Name = "Performance")]
        Operation = 6,
        [Display(Name = "HSE")]
        HSE = 7,
        [Display(Name = "Central Warehouse Material")]
        WarehouseMaterial = 8
    }
}
