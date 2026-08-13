using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using Nexora.PMPortal.Projects;
using System.Threading.Tasks;
using MD.PersianDateTime;
using Abp.Application.Services.Dto;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ProjectPlanForm
{
    public class ProjectPlanFormViewComponent : PMPortalViewComponent
    {
        private readonly IProjectPlanAppService _projectPlanAppService;

        public ProjectPlanFormViewComponent(IProjectPlanAppService projectPlanAppService)
        {
            _projectPlanAppService = projectPlanAppService;
        }

        public IViewComponentResult Invoke(int? reportId, int projectId)
        {
            var model = new ProjectPlanFormViewModel();
            model.ProjectId = projectId;
            if (reportId != null)
            {
                var record = _projectPlanAppService.Get(new EntityDto<int>(reportId.Value)).Result;
                model.Id = record.Id;
                model.Description = record.Description;
                model.StartDate = new PersianDateTime(record.StartDate).ToShortDateString();
                model.EndDate = new PersianDateTime(record.EndDate).ToShortDateString();
                model.FileUrl = record.FileUrl;
                model.ProjectId = record.ProjectId;
                model.ProjectPlanType = record.ProjectPlanType;
            }
            return View(model);

        }
    }
}
