using Nexora.PMPortal.Enums;
using Nexora.PMPortal.ProjectsDocumentations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexora.PMPortal.Web.ViewModels
{
    public class DashboardDocsStatusViewModel
    {
        public string DiciplineType { get; set; }
        public string DocumentType { get; set; }

        //Action Types
        public int? IssuedByPM { get; set; }
        public int? CommentedByFSTCO { get; set; }
        public int? CommentedByIDOM { get; set; }
        public int? ApprovedByFSTCO { get; set; }
        public int? ApprovedByIDOM { get; set; }
        public int? AppWithNotesByFSTCO { get; set; }
        public int? AppWithNotesByIDOM { get; set; }
        public int? NotIssued { get; set; }
        public int? Delete { get; set; }
        public int? Total { get; set; }
        public int? TotalIssued { get; set; }

        public delegate List<DashboardDocsStatusViewModel> DocumentTypeCounter(string Titel , List<ProjectsTasks> List);

        public delegate List<DashboardDocsStatusViewModel> ActionTypeCounter(List<ProjectsTasks> List);
    }
}
