using System.Collections.Generic;
using Abp.Localization;
using Nexora.PMPortal.Enums;

namespace Nexora.PMPortal.Web.Views.Shared.Components.FinancialStatementForm
{
    public class FinancialStatementFormViewModel
    {
        public int? Id { get; set; }
        public FinancialStatementType FinancialStatementType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string Number { get; set; }
        public string Period { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PeriodicAmount { get; set; }
        public string FileUrl { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ProjectId { get; set; }
    }

}
