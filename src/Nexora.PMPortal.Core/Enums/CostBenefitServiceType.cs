using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{

    public enum CostBenefitServiceType
    {
        [Display(Name = "Utility")]
        Utilty = 1,
        [Display(Name = "Industrial Development")]
        IndustrialDevelopment = 2,
        [Display(Name = "Electricity & Energy")]
        ElectricityAndEnergy = 3,
        [Display(Name = "Water Supply & Distribution")]
        WaterSupply = 4,
        [Display(Name = "Water & Wastewater")]
        WaterAndWasteWater = 5
    }
}
