using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Abp.Localization;
using Nexora.PMPortal.Web.Views.Shared.Components.PaymentRequestTransaction;
using Nexora.PMPortal.Projects;
using System.Threading.Tasks;
using System;
using MD.PersianDateTime;
using Nexora.PMPortal.Web.ViewModels;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ProjectsTasksRevisions
{
    public class ProjectsTasksRevisionsViewComponent : PMPortalViewComponent
    {
        public IViewComponentResult Invoke(string ProjectCode , int TaskID , string DocumentNumber , string DocumentTitle)
        {
            var model = new ProjectsTasksRevisionsViewModel
            {
                ProjectCode = ProjectCode,

                TaskID = TaskID,

                DocumentNumber = DocumentNumber,

                DocumentTitle = DocumentTitle
            };
            return View(model);
        }
    }
}
