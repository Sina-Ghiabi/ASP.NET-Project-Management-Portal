using Abp.Application.Navigation;
using Abp.Localization;
using Nexora.PMPortal.Authorization;
using System.Collections.Generic;

namespace Nexora.PMPortal.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class PMPortalNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        new FixedLocalizableString("Dashboard"),
                        url: "",
                        icon: "home",
                        requiredPermissionName: "PmPortal.Dashboard"
                    )
                )
                .AddItem(
                        new MenuItemDefinition(
                             PageNames.Projects,
                             new FixedLocalizableString("Projects"),
                             icon: "dns",
                             url: "Project",
                             requiredPermissionName: "PmPortal.Project"))
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Projects,
                        new FixedLocalizableString("Project Reports"),
                        icon: "dns",
                        requiredPermissionName: "PmPortal.ProjectsReports"
                    ).AddItem(
                        new MenuItemDefinition(
                             PageNames.ProjectsTasks,
                             new FixedLocalizableString("Engineering Reports"),
                             url: "ProjectsDocumentations/ProjectsTasks",
                             requiredPermissionName: "PmPortal.ProjectsReports.ProjectsTasks"
                        ))

                ).AddItem( // Menu items below is just for demonstration!
                    new MenuItemDefinition(
                        "MultiLevelMenu0",
                        new FixedLocalizableString("Liquidity Management"),
                        icon: "payment",
                        requiredPermissionName: "PmPortal.Liquidity"
                    ).AddItem(
                        new MenuItemDefinition(
                             PageNames.PaymentRequests,
                             new FixedLocalizableString("Payment Requests"),
                             url: "Financial/PaymentRequests",
                             requiredPermissionName: "PmPortal.Liquidity.PaymentRequests"
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                             PageNames.ReceiveBills,
                             new FixedLocalizableString("Receive Bills"),
                             url: "Financial/ReceiveBills",
                             requiredPermissionName: "PmPortal.Liquidity.ReceiveBills"
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.Transactions,
                            new FixedLocalizableString("Transactions"),
                            url: "Financial/Transactions",
                            requiredPermissionName: "PmPortal.Liquidity.Transactions"
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.RemainCredit,
                            new FixedLocalizableString("Remaining Credit"),
                            url: "Financial/RemainCredit",
                            requiredPermissionName: "PmPortal.Liquidity.RemainCredit"
                        )
                    )
                 ).AddItem( // Menu items below is just for demonstration!
                    new MenuItemDefinition(
                        "MultiLevelMenu1",
                        new FixedLocalizableString("Reports"),
                        icon: "assignment",
                        requiredPermissionName: "PmPortal.Reports"
                    ).AddItem(
                        new MenuItemDefinition(
                             PageNames.ResourceAndUsage,
                             new FixedLocalizableString("Resources and Usage"),
                             url: "Reports/ResourceAndUsage",
                             requiredPermissionName: "PmPortal.Reports.ResourceAndUsage"
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                           PageNames.ManagerReports,
                             new FixedLocalizableString("Management"),
                             url: "ManagerReport",
                             requiredPermissionName: "PmPortal.Reports.ManagerReports"
                        )
                    )
                   ).AddItem( // Menu items below is just for demonstration!
                    new MenuItemDefinition(
                        "MultiLevelMenu1",
                        new FixedLocalizableString("Information Forms"),
                        icon: "assignment",
                        requiredPermissionName: "PmPortal.InformationForm"
                    )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.CostBenefit,
                                new FixedLocalizableString("Sales & Profit/Loss"),
                                url: "CostBenefit/Index",
                                requiredPermissionName: "PmPortal.InformationForm.CostBenefit"

                            )
                         )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.ReceiveAndPayment,
                                new FixedLocalizableString("Receive and Payment"),
                                url: "ReceiveAndPayment/Index",
                                requiredPermissionName: "PmPortal.InformationForm.ReceiveAndPayment"
                            )
                         )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.GrossProfitToNet,
                                new FixedLocalizableString("Gross to Net Profit"),
                                url: "GrossProfitToNet/Index",
                                requiredPermissionName: "PmPortal.InformationForm.GrossProfitToNet"
                            )
                         )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.WarrantyBalance,
                                new FixedLocalizableString("Warranty"),
                                url: "WarrantyBalance/Index",
                                requiredPermissionName: "PmPortal.InformationForm.WarrantyBalance"
                            )
                         )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.ProgressPercentage,
                                new FixedLocalizableString("Progress Percentage"),
                                url: "ProgressPercentage/Index",
                                requiredPermissionName: "PmPortal.InformationForm.ProgressPercentage"
                            )
                         )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.LaborInventory,
                                new FixedLocalizableString("Human Resources"),
                                url: "LaborInventory/Index",
                                requiredPermissionName: "PmPortal.InformationForm.LaborInventory"
                            )
                         )
                     ).AddItem( // Menu items below is just for demonstration!
                         new MenuItemDefinition(
                             "MultiLevelMenu1",
                             new FixedLocalizableString("Basic Information"),
                             icon: "assignment"
                         //requiredPermissionName: "PmPortal.Reports"
                           ).AddItem(
                            new MenuItemDefinition(
                                PageNames.Contractors,
                                new FixedLocalizableString("Contractors"),
                                url: "Financial/Contractors",
                                requiredPermissionName: "PmPortal.Contractor"

                            )
                           ).AddItem(
                           new MenuItemDefinition(
                               PageNames.Users,
                               new FixedLocalizableString("Users"),
                               url: "Users",
                               requiredPermissionName: "PmPortal.User"
                               )
                          )
                          .AddItem(
                              new MenuItemDefinition(
                                  PageNames.Roles,
                                  new FixedLocalizableString("Roles"),
                                  url: "Roles",
                                  requiredPermissionName: "PmPortal.Role"
                              )
                          )
                 );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, PMPortalConsts.LocalizationSourceName);
        }
    }
}
