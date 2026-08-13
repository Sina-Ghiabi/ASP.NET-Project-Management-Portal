using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nexora.PMPortal.Enums
{
    public enum ProjectsTasksDocumentTypes
    {
        [Display(Name = "Data Sheet")]
        DataSheet = 1,
        [Display(Name = "Material Requisition")]
        MaterialRequisition = 2,
        [Display(Name = "Technical Bid Evaluation")]
        TechnicalBidEvaluation = 3,
        [Display(Name = "Vendor Data & Drawing")]
        VendorDataAndDrawing = 4,
        [Display(Name = "Diagram")]
        Diagram = 5,
        [Display(Name = "Specification")]
        Specification = 6,
        [Display(Name = "Calculation List")]
        CalculationList = 7,
        [Display(Name = "Manual Procedure")]
        ManualProcedure = 8,
        [Display(Name = "Material Take Off")]
        MaterialTakeOff = 9,
        [Display(Name = "Report List")]
        ReportList = 10,
        [Display(Name = "Basic Design")]
        BasicDesign = 11,
        [Display(Name = "Detail Design")]
        DetailDesign = 12,
        [Display(Name = "Procurement Engineering")]
        ProcurementEngineering = 13
    }
}
