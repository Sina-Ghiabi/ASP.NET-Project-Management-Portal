using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Nexora.PMPortal.Authorization
{
    public class PMPortalAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            //context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            //context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            //Dashboard
            var pmportal = context.CreatePermission("PmPortal.Dashboard", new FixedLocalizableString("Dashboard"));
            pmportal.CreateChildPermission("PmPortal.Dashboard.Projects", new FixedLocalizableString("Projects Dashboard"));
            pmportal.CreateChildPermission("PmPortal.Dashboard.ProjectsDocumentations", new FixedLocalizableString("Engineering Dashboard"));
            pmportal.CreateChildPermission("PmPortal.Dashboard.Allocation", new FixedLocalizableString("Allocation Dashboard"));
            pmportal.CreateChildPermission("PmPortal.Dashboard.Managers", new FixedLocalizableString("Management Dashboard"));

            //Projects
            var project = pmportal.CreateChildPermission("PmPortal.Project", new FixedLocalizableString("Projects"));
            project.CreateChildPermission("PmPortal.Project.Create", new FixedLocalizableString("Add New Project"));
            project.CreateChildPermission("PmPortal.Project.Details", new FixedLocalizableString("Project Details"));
            project.CreateChildPermission("PmPortal.Project.Edit", new FixedLocalizableString("Edit Project"));
            project.CreateChildPermission("PmPortal.Project.Delete", new FixedLocalizableString("Delete Project"));

            //Project Reports
            var projectsreports = pmportal.CreateChildPermission("PmPortal.ProjectsReports", new FixedLocalizableString("Project Reports"));
            var projectstasks = projectsreports.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks", new FixedLocalizableString("Engineering Reports"));
            var projectstaskRevisions = projectsreports.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks.ProjectsTaskRevisions", new FixedLocalizableString("Engineering Report Revisions"));

            //Engineering Reports
            projectstasks.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks.Create", new FixedLocalizableString("Add New Engineering Report"));
            projectstasks.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks.Edit", new FixedLocalizableString("Edit Engineering Report"));
            projectstasks.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks.Schedule", new FixedLocalizableString("Engineering Report Schedule"));
            projectstasks.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks.Delete", new FixedLocalizableString("Delete Engineering Report"));

            //Engineering Report Revisions
            projectstaskRevisions.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks.ProjectsTaskRevisions.Create", new FixedLocalizableString("Add Revision"));
            projectstaskRevisions.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks.ProjectsTaskRevisions.Edit", new FixedLocalizableString("Edit Revision"));
            projectstaskRevisions.CreateChildPermission("PmPortal.ProjectsReports.ProjectsTasks.ProjectsTaskRevisions.Delete", new FixedLocalizableString("Delete Revision"));


            var financialStatement = project.CreateChildPermission("PmPortal.Project.FinancialStatement", new FixedLocalizableString("Financial Statements"));
            financialStatement.CreateChildPermission("PmPortal.Project.FinancialStatement.Create", new FixedLocalizableString("Add New Financial Statement"));
            financialStatement.CreateChildPermission("PmPortal.Project.FinancialStatement.Edit", new FixedLocalizableString("Edit Financial Statement"));
            financialStatement.CreateChildPermission("PmPortal.Project.FinancialStatement.Delete", new FixedLocalizableString("Delete Financial Statement"));


            var projectReport = project.CreateChildPermission("PmPortal.Project.ProjectReport", new FixedLocalizableString("Reports"));
            projectReport.CreateChildPermission("PmPortal.Project.ProjectReport.Create", new FixedLocalizableString("Add New Report"));
            projectReport.CreateChildPermission("PmPortal.Project.ProjectReport.Edit", new FixedLocalizableString("Edit Report"));
            projectReport.CreateChildPermission("PmPortal.Project.ProjectReport.Delete", new FixedLocalizableString("Delete Report"));

            var projectPlan = project.CreateChildPermission("PmPortal.Project.ProjectPlan", new FixedLocalizableString("Plans"));
            projectPlan.CreateChildPermission("PmPortal.Project.ProjectPlan.Create", new FixedLocalizableString("Add New Plan"));
            projectPlan.CreateChildPermission("PmPortal.Project.ProjectPlan.Edit", new FixedLocalizableString("Edit Plan"));
            projectPlan.CreateChildPermission("PmPortal.Project.ProjectPlan.Delete", new FixedLocalizableString("Delete Plan"));


            var projectMedia = project.CreateChildPermission("PmPortal.Project.ProjectMedia", new FixedLocalizableString("Images"));
            projectMedia.CreateChildPermission("PmPortal.Project.ProjectMedia.Create", new FixedLocalizableString("Add New Image"));
            projectMedia.CreateChildPermission("PmPortal.Project.ProjectMedia.Delete", new FixedLocalizableString("Delete Image"));


            var projectAsset = project.CreateChildPermission("PmPortal.Project.ProjectAsset", new FixedLocalizableString("Assets"));
            projectAsset.CreateChildPermission("PmPortal.Project.ProjectAsset.Create", new FixedLocalizableString("Add New File"));

            var projectGallery = project.CreateChildPermission("PmPortal.Project.ProjectGallery", new FixedLocalizableString("Gallery"));


            //Liquidity Management
            var liquidity = pmportal.CreateChildPermission("PmPortal.Liquidity", new FixedLocalizableString("Liquidity Management"));

            //Payment Requests
            var paymentRequests = liquidity.CreateChildPermission("PmPortal.Liquidity.PaymentRequests", new FixedLocalizableString("Payment Requests"));
            paymentRequests.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.Create", new FixedLocalizableString("Add New Payment Request"));
            paymentRequests.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.Transactions", new FixedLocalizableString("Payment Request Transactions"));
            paymentRequests.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.Edit", new FixedLocalizableString("Edit Payment Request"));
            paymentRequests.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.EditAnyWay", new FixedLocalizableString("Edit Finalized Payment Request"));
            paymentRequests.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.Delete", new FixedLocalizableString("Delete Payment Request"));
            //paymentRequests.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.", new FixedLocalizableString("Delete Payment Request"));


            //Project Manager Approval Queue
            var paymentRequestsQueueOne = liquidity.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueOne", new FixedLocalizableString("Project Manager Approval Queue"));
            paymentRequestsQueueOne.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueOne.Confirm", new FixedLocalizableString("Approve Requests"));
            paymentRequestsQueueOne.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueOne.Decline", new FixedLocalizableString("Decline Requests"));
            paymentRequestsQueueOne.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueOne.Edit", new FixedLocalizableString("Edit Payment Request"));

            //Planning Approval Queue
            var paymentRequestsQueueTwo = liquidity.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueTwo", new FixedLocalizableString("Planning Approval Queue"));
            paymentRequestsQueueTwo.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueTwo.Transactions", new FixedLocalizableString("Transactions & Approve Payment Request"));
            paymentRequestsQueueTwo.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueTwo.NotAcceptable", new FixedLocalizableString("Not Approved"));
            paymentRequestsQueueTwo.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueTwo.Decline", new FixedLocalizableString("Decline Requests"));
            paymentRequestsQueueTwo.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.QueueTwo.Edit", new FixedLocalizableString("Edit Payment Request"));

            //Rejected Payment Requests
            var paymentRequestsDeclined = liquidity.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.Declined", new FixedLocalizableString("Rejected Payment Requests"));
            paymentRequestsDeclined.CreateChildPermission("PmPortal.Liquidity.PaymentRequests.Declined.Delete", new FixedLocalizableString("Delete Payment Request"));

            //Receive Bills
            var receiveBills = liquidity.CreateChildPermission("PmPortal.Liquidity.ReceiveBills", new FixedLocalizableString("Receive Bills"));
            receiveBills.CreateChildPermission("PmPortal.Liquidity.ReceiveBills.Create", new FixedLocalizableString("Add New Receive Bill"));
            receiveBills.CreateChildPermission("PmPortal.Liquidity.ReceiveBills.Transactions", new FixedLocalizableString("Receive Bill Transactions"));
            receiveBills.CreateChildPermission("PmPortal.Liquidity.ReceiveBills.Edit", new FixedLocalizableString("Edit Receive Bill"));
            receiveBills.CreateChildPermission("PmPortal.Liquidity.ReceiveBills.Delete", new FixedLocalizableString("Delete Receive Bill"));

            //Transactions
            var transactinos = liquidity.CreateChildPermission("PmPortal.Liquidity.Transactions", new FixedLocalizableString("Transactions"));

            //Remaining Credit
            var RemainCredit = liquidity.CreateChildPermission("PmPortal.Liquidity.RemainCredit", new FixedLocalizableString("Remaining Credit"));
            RemainCredit.CreateChildPermission("PmPortal.Liquidity.RemainCredit.ImportExcel", new FixedLocalizableString("Import Excel"));

            //Contractors
            var contractor = pmportal.CreateChildPermission("PmPortal.Contractor", new FixedLocalizableString("Contractors"));
            contractor.CreateChildPermission("PmPortal.Contractor.Create", new FixedLocalizableString("Add New Contractor"));
            contractor.CreateChildPermission("PmPortal.Contractor.Edit", new FixedLocalizableString("Edit Contractor"));
            contractor.CreateChildPermission("PmPortal.Contractor.Delete", new FixedLocalizableString("Delete Contractor"));


            //Reports
            var reports = pmportal.CreateChildPermission("PmPortal.Reports", new FixedLocalizableString("Reports"));

            var resourceAndUsage = reports.CreateChildPermission("PmPortal.Reports.ResourceAndUsage", new FixedLocalizableString("Resources and Usage"));

            var managerReports = reports.CreateChildPermission("PmPortal.Reports.ManagerReports", new FixedLocalizableString("Management"));
            managerReports.CreateChildPermission("PmPortal.Reports.ManagerReports.Create", new FixedLocalizableString("Add New Report"));
            managerReports.CreateChildPermission("PmPortal.Reports.ManagerReports.Edit", new FixedLocalizableString("Edit Report"));
            managerReports.CreateChildPermission("PmPortal.Reports.ManagerReports.Delete", new FixedLocalizableString("Delete Report"));


            //Information Forms
            var infromationForm = pmportal.CreateChildPermission("PmPortal.InformationForm", new FixedLocalizableString("Information Forms"));

            infromationForm.CreateChildPermission("PmPortal.InformationForm.CostBenefit", new FixedLocalizableString("Sales & Profit/Loss"));
            infromationForm.CreateChildPermission("PmPortal.InformationForm.ReceiveAndPayment", new FixedLocalizableString("Receive and Payment"));
            infromationForm.CreateChildPermission("PmPortal.InformationForm.GrossProfitToNet", new FixedLocalizableString("Gross to Net Profit"));
            infromationForm.CreateChildPermission("PmPortal.InformationForm.WarrantyBalance", new FixedLocalizableString("Warranty"));
            infromationForm.CreateChildPermission("PmPortal.InformationForm.ProgressPercentage", new FixedLocalizableString("Progress Percentage"));
            infromationForm.CreateChildPermission("PmPortal.InformationForm.LaborInventory", new FixedLocalizableString("Human Resources"));



            //Users
            var user = pmportal.CreateChildPermission("PmPortal.User", new FixedLocalizableString("Users"));
            user.CreateChildPermission("PmPortal.User.Create", new FixedLocalizableString("Add New User"));
            user.CreateChildPermission("PmPortal.User.Edit", new FixedLocalizableString("Edit User"));
            user.CreateChildPermission("PmPortal.User.ResetPassword", new FixedLocalizableString("Reset User Password"));
            user.CreateChildPermission("PmPortal.User.Delete", new FixedLocalizableString("Delete User"));

            //Roles
            var role = pmportal.CreateChildPermission("PmPortal.Role", new FixedLocalizableString("Roles"));
            role.CreateChildPermission("PmPortal.Role.Create", new FixedLocalizableString("Add New Role"));
            role.CreateChildPermission("PmPortal.Role.Edit", new FixedLocalizableString("Edit Role"));
            role.CreateChildPermission("PmPortal.Role.Delete", new FixedLocalizableString("Delete Role"));


        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, PMPortalConsts.LocalizationSourceName);
        }
    }
}
