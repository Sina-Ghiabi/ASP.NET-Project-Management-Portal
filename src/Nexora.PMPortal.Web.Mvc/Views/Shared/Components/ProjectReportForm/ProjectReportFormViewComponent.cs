using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using Nexora.PMPortal.Projects;
using System.Threading.Tasks;
using MD.PersianDateTime;
using Abp.Application.Services.Dto;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ProjectReportForm
{
    public class ProjectReportFormViewComponent : PMPortalViewComponent
    {
        private readonly IProjectReportAppService _projectReportAppService;

        public ProjectReportFormViewComponent(IProjectReportAppService projectReportAppService)
        {
            _projectReportAppService = projectReportAppService;
        }

        public IViewComponentResult Invoke(int? reportId, int projectId)
        {
            var model = new ProjectReportFormViewModel();
            model.ProjectId = projectId;
            if (reportId != null)
            {
                var record = _projectReportAppService.Get(new EntityDto<int>(reportId.Value)).Result;
                model.Id = record.Id;
                model.Description = record.Description;
                model.StartDate = new PersianDateTime(record.StartDate).ToShortDateString();
                model.EndDate = new PersianDateTime(record.EndDate).ToShortDateString();
                model.FileUrl = record.FileUrl;
                model.ProjectId = record.ProjectId;
                model.ProjectReportType = record.ProjectReportType;
            }
            return View(model);

        }
    }
}
