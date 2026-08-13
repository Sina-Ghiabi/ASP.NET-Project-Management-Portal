using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectsTasksActionTypes
    {
        [Display(Name = "Approved By FSTCO")]
        ApprovedByFSTCO = 1,
        [Display(Name = "Approved By IDOM")]
        ApprovedByIDOM = 2,
        [Display(Name = "Commented By FSTCO")]
        CommentedByFSTCO = 3,
        [Display(Name = "Commented By IDOM")]
        CommentedByIDOM = 4,
        [Display(Name = "Issued By PM")]
        IssuedByPM = 5,
        [Display(Name = "Not Issue")]
        NotIssue = 6,
        [Display(Name = "Delete")]
        Delete = 7,

        [Display(Name = "Approved")]
        Approved = 8,
        [Display(Name = "Conditionally Approved")]
        ConditionalApproved = 9,
        [Display(Name = "Under Review by PM")]
        PMChecking = 10,
        [Display(Name = "Under Review by FSTCO")]
        FSTCOChecking = 11,
        [Display(Name = "Under Review by Employer")]
        EmployerChecking= 12,
        [Display(Name = "Under Review by Vendor")]
        VendorChecking = 13,
        [Display(Name = "Deleted")]
        FaDelete = 14,
        [Display(Name = "-----")]
        FaEmpty = 15,

        [Display(Name = "App With Notes By FSTCO")]
        AppWithNotesByFSTCO = 16,
        [Display(Name = "App With Notes By IDOM")]
        AppWithNotesByIDOM = 17,
    }
}
