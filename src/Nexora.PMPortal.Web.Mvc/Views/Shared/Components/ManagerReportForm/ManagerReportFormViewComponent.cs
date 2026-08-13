using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using Nexora.PMPortal.Projects;
using System.Threading.Tasks;
using MD.PersianDateTime;
using Abp.Application.Services.Dto;
using Nexora.PMPortal.Managers;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ManagerReportForm
{
    public class ManagerReportFormViewComponent : PMPortalViewComponent
    {
        private readonly IManagerReportAppService _ManagerReportAppService;

        public ManagerReportFormViewComponent(IManagerReportAppService managerReportAppService)
        {
            _ManagerReportAppService = managerReportAppService;
        }

        public IViewComponentResult Invoke(int managerReportId)
        {
            var model = new ManagerReportFormViewModel();
            var record = _ManagerReportAppService.Get(new EntityDto<int>(managerReportId)).Result;

            model.Id = record.Id;
            model.Description = record.Description;
            model.StartDate = new PersianDateTime(record.StartDate).ToShortDateString();
            model.EndDate = new PersianDateTime(record.EndDate).ToShortDateString();
            model.FileUrl = record.FileUrl;

            return View(model);

        }
    }
}
