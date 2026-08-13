using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using MD.PersianDateTime;
using System;
using Nexora.PMPortal.Projects;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.Views.Shared.Components.UserProjectModal
{
    public class UserProjectModalViewComponent : PMPortalViewComponent
    {
        private readonly IProjectAppService _ProjectAppService;

        public UserProjectModalViewComponent(IProjectAppService ProjectAppService)
        {
            _ProjectAppService = ProjectAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(long userId)
        {
            var model = new UserProjectModalViewModel
            {
                UserId = userId
            };

            var items = await _ProjectAppService.GetAllProjects();
            ViewBag.ProjectId = items;

            return View(model);
        }
    }
}
