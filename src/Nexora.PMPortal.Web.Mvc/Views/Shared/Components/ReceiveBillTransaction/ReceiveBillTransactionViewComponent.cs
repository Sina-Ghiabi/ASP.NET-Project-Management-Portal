using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using Nexora.PMPortal.Projects;
using System.Threading.Tasks;
using System;
using MD.PersianDateTime;
using Nexora.PMPortal.Web.ViewModels;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ReceiveBillTransaction
{
    public class ReceiveBillTransactionViewComponent : PMPortalViewComponent
    {
        private readonly IProjectAppService _projectAppService;
        private readonly IProject_User_MappingAppService _project_User_MappingAppService;

        public ReceiveBillTransactionViewComponent(IProjectAppService projectAppService, IProject_User_MappingAppService project_User_MappingAppService)
        {
            _projectAppService = projectAppService;
            _project_User_MappingAppService = project_User_MappingAppService;

        }

        public IViewComponentResult Invoke(int ReceivebillId)
        {
            var model = new ReceiveBillTransactionViewModel
            {
                Id = ReceivebillId,
                TodayDate = new PersianDateTime(DateTime.Now).ToShortDateString()
            };
            var userId = AbpSession.UserId.Value;
            var items = _project_User_MappingAppService.GetUserProjects(userId).Result;
            ViewBag.ProjectId = items.Select(a => new ProjectViewModel()
            {
                Id = a.ProjectId,
                Title = a.Project.Title
            }).ToList();
            return View(model);
        }
    }
}
