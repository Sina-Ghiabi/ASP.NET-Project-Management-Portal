using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum PaymentRequestType
    {
        [Display(Name = "Payment to Contractors")]
        PayingToContractors = 1,
        [Display(Name = "Supply of Mechanical, Electrical, Structural and Piping Equipment")]
        SupplyOfEquipment = 2,
        [Display(Name = "Purchase of Materials")]
        BuyingMaterials = 3,
        [Display(Name = "Site Support and Equipping")]
        SupportAndEquipWorkshop = 4,
        [Display(Name = "Petty Cash")]
        Fund = 5,
        [Display(Name = "Headquarters")]
        HeadQuarter = 6,
        [Display(Name = "Guarantee")]
        Warranty = 7,
        [Display(Name = "Currency Purchase")]
        BuyCurrency = 8
    }
}
