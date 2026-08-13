using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using Nexora.PMPortal.Projects;
using System.Threading.Tasks;
using MD.PersianDateTime;
using Abp.Application.Services.Dto;

namespace Nexora.PMPortal.Web.Views.Shared.Components.FinancialStatementForm
{
    public class FinancialStatementFormViewComponent : PMPortalViewComponent
    {
        private readonly IFinancialStatementAppService _financialStatementAppService;

        public FinancialStatementFormViewComponent(IFinancialStatementAppService financialStatementAppService)
        {
            _financialStatementAppService = financialStatementAppService;
        }

        public IViewComponentResult Invoke(int? statementId, int projectId)
        {
            var model = new FinancialStatementFormViewModel();
            model.ProjectId = projectId;
            if (statementId != null)
            {
                var record = _financialStatementAppService.Get(new EntityDto<int>(statementId.Value)).Result;
                model.Id = record.Id;
                model.FinancialStatementType = record.FinancialStatementType;
                model.CurrencyType = record.CurrencyType;
                model.Number = record.Number;
                model.Period = record.Period;
                model.TotalAmount = record.TotalAmount;
                model.PeriodicAmount = record.PeriodicAmount;
                model.FileUrl = record.FileUrl;
                model.StartDate = new PersianDateTime(record.StartDate).ToShortDateString();
                model.EndDate = new PersianDateTime(record.EndDate).ToShortDateString();
                model.ProjectId = record.ProjectId;
            }
            return View(model);

        }
    }
}
