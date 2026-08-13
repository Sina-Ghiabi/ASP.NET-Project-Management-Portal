using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using Nexora.PMPortal.Projects;
using System.Threading.Tasks;
using System;
using MD.PersianDateTime;
using Nexora.PMPortal.Web.ViewModels;
using Nexora.PMPortal.Web.Views.Shared.Components.ProjectsTasksFilterColumns;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ProjectsTasksRevisions
{
    public class ProjectsTasksFilterColumnsViewComponent : PMPortalViewComponent
    {
        public IViewComponentResult Invoke(ProjectsTasksFilterColumnsViewModel List)
        {
            return View(List);
        }
    }
}
