using Nexora.PMPortal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.Views.Shared.Components.ProjectsTasksFilterColumns
{
    public class ProjectsTasksFilterColumnsViewModel
    {

        public int? CreatorUserID { get; set; }

        //ProjectsTask Fields
        public int? TaskID { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string CompanyName { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentNumber { get; set; }
        public ProjectsTasksDiciplineTypes? Dicipline { get; set; }
        public string ResponsiblePerson { get; set; }
        public ProjectsTasksDocumentTypes? DocumentType { get; set; }
        public string Description { get; set; }

        //ProjectsTaskSchedule Fields
        public int? Progress { get; set; }
        public string BaseLineStart { get; set; }
        public string BaseLineFinished { get; set; }
        public int? OriginalDuration { get; set; }
        public ProjectsTasksSourceOfItemTypes? SourceOfItem { get; set; }
        public int? ManPower { get; set; }
        public bool Critical { get; set; }

        //Fields Below Will Be Filled From Last Revision 
        public string TransmitalNumber { get; set; }
        public string TransmitalDate { get; set; }
        public string StartTransmitalDate { get; set; }
        public string EndTransmitalDate { get; set; }
        public string CommentSheetNumber { get; set; }
        public string CommentSheetDate { get; set; }
        public string ReplySheetNumber { get; set; }
        public string ReplySheetDate { get; set; }
        public string RevisionNumber { get; set; }
        public int? Status { get; set; }
        public int? Action { get; set; }
    }
}
