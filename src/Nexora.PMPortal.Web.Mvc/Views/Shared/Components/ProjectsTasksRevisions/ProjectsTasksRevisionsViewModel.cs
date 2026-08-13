using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ProjectsTasksRevisions
{
    public class ProjectsTasksRevisionsViewModel
    {
        public string ProjectCode { get; set; }
        public int TaskID { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentTitle { get; set; }
    }
}
