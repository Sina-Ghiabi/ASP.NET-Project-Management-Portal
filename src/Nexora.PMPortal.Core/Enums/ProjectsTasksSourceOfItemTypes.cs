using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectsTasksSourceOfItemTypes
    {
        [Display(Name = "MDL Basic")]
        MDLBasic = 1,
        [Display(Name = "MDL Detail")]
        MDLDetail = 2,
        [Display(Name = "MDL Procurement")]
        MDLProcurement = 3,
        [Display(Name = "Task")]
        Task = 4,
        [Display(Name = "Vendor Document")]
        VendorDocument = 5,
        [Display(Name = "Bid Document")]
        BidDocument = 6,
        [Display(Name = "Sub Vendor")]
        SubVendor = 7
    }
}
