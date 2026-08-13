using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Nexora.PMPortal.Authorization.Roles;
using Nexora.PMPortal.Authorization.Users;
using Nexora.PMPortal.MultiTenancy;
using Nexora.PMPortal.Projects;
using Nexora.PMPortal.Financial;
using Nexora.PMPortal.Managers;
using Nexora.PMPortal.ProjectsDocumentations;

namespace Nexora.PMPortal.EntityFrameworkCore
{
    public class PMPortalDbContext : AbpZeroDbContext<Tenant, Role, User, PMPortalDbContext>
    {
        public virtual DbSet<Katibe_Darkhast> Katibe_DarkhastPardakhtVajh { get; set; }

        /* Define a DbSet for each entity of the application */
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectMedia> ProjectMedias { get; set; }
        public virtual DbSet<FinancialStatement> FinancialStatements { get; set; }
        public virtual DbSet<ProjectReport> ProjectReports { get; set; }
        public virtual DbSet<ProjectPlan> ProjectPlans { get; set; }
        public virtual DbSet<ChosenColumns> ChosenColumns { get; set; }
        public virtual DbSet<ProjectReportFile> ProjectReportFiles { get; set; }
        public virtual DbSet<ProjectPlanFile> ProjectPlanFiles { get; set; }
        public virtual DbSet<Project_User_Mapping> Project_User_Mappings { get; set; }



        public virtual DbSet<PaymentRequest> PaymentRequests { get; set; }
        public virtual DbSet<ReceiveBill> ReceiveBills { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<RemainCredit> RemainCredits { get; set; }
        public virtual DbSet<ManagerReport> ManagerReports { get; set; }
        public virtual DbSet<Contractor> Contractors { get; set; }



        public virtual DbSet<CostBenefit> CostBenefits { get; set; }
        public virtual DbSet<GrossProfitToNet> GrossProfitToNets { get; set; }
        public virtual DbSet<LaborInventory> LaborInventories { get; set; }
        public virtual DbSet<ReceiveAndPayment> ReceiveAndPayments { get; set; }
        public virtual DbSet<WarrantyBalance> WarrantyBalances { get; set; }
        public virtual DbSet<ProgressPercentage> ProgressPercentages { get; set; }


        public virtual DbSet<ProjectsTasks> ProjectsTasks { get; set; }
        public virtual DbSet<ProjectsTasksRevisions> ProjectsTasksRevisions { get; set; }

        public PMPortalDbContext(DbContextOptions<PMPortalDbContext> options)
            : base(options)
        {
        }
    }
}
