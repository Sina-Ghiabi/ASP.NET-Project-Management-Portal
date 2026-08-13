using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class FinancialStatementViewModel
    {
        public int Id { get; set; }
        public FinancialStatementType FinancialStatementType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string Number { get; set; }
        public string Period { get; set; }
        public string TotalAmount { get; set; }
        public string PeriodicAmount { get; set; }
        public string FileUrl { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ProjectId { get; set; }
    }
}
