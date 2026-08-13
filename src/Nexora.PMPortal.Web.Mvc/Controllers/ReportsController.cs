using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Nexora.PMPortal.Controllers;
using Nexora.PMPortal.Enums;
using Nexora.PMPortal.Financial;
using Nexora.PMPortal.Projects;
using Nexora.PMPortal.Web.ViewModels;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Mvc;

namespace Nexora.PMPortal.Web.Mvc.Controllers
{
    [AbpMvcAuthorize]
    public class ReportsController : PMPortalControllerBase
    {

        private readonly IPaymentRequestAppService _paymentRequestAppService;
        private readonly IReceiveBillAppService _receiveBillAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IProjectAppService _projectAppService;

        public ReportsController(IPaymentRequestAppService paymentRequestAppService,
            IReceiveBillAppService receiveBillAppService,
            ITransactionAppService transactionAppService,
            IProjectAppService projectAppService
            )
        {
            _paymentRequestAppService = paymentRequestAppService;
            _receiveBillAppService = receiveBillAppService;
            _transactionAppService = transactionAppService;
            _projectAppService = projectAppService;
        }


        public async Task<IActionResult> ResourceAndUsage(int? currencyId, int? yearId)
        {
            var model = new ResourceAndUsageViewModel();

            if (yearId != null)
            {
                model.ReportYear = (ReportYear)yearId;                
            }
            else
            {
                model.ReportYear = (ReportYear)new PersianDateTime(DateTime.Now).Year;
                yearId = new PersianDateTime(DateTime.Now).Year;
            }

            var projects = await _projectAppService.GetAllProjects();

            var transactions = await _transactionAppService.GetAllTransactionsForUsageReport(currencyId, yearId);

            var receiveBills = await _receiveBillAppService.GetAllReceiveBillsForUsageReport(yearId);

            var payments = transactions.Where(a => a.TransactionType == TransactionType.Payment).ToList();



            if (currencyId != null)
            {
                model.CurrencyType = (CurrencyType)currencyId;
            }

            

            var income = receiveBills.Sum(a => a.Amount);
            var totaltakhsis = transactions.Where(a => a.TransactionType == TransactionType.Receive).Sum(a => a.Amount);

            model.TotalSave = (income - totaltakhsis) / 1000000;

            model.Projects = projects.Select(a => new ProjectViewModel()
            {
                Id = a.Id,
                Title = a.Title,
                ShowInUsageReport = a.ShowInUsageReport,
                UsageReportDisplayOrder = a.UsageReportDisplayOrder
            }).ToList();

            var rows = new List<ResourceAndUsageRowViewModel>();

            foreach (var item in transactions)
            {
                var currentCreationTime = item.CreationTime;
                var currentTransactionType = item.TransactionType;

                var append = new ResourceAndUsageRowViewModel();
                append.DateTime = currentCreationTime;
                append.TransactionType = item.TransactionType;

                if (!rows.Any(a => a.DateTime.Date == currentCreationTime.Date && a.TransactionType == currentTransactionType))
                {
                    foreach (var project in projects)
                    {
                        append.ProjectUsages.Add(new ResourceAndUsageColumnViewModel()
                        {
                            ProjectId = project.Id,
                            ProjectName = project.Title,
                            ShowInTable = project.ShowInUsageReport,
                            DisplayOrder = project.UsageReportDisplayOrder,
                            TodayTotalAmount = transactions.Where(b => b.CreationTime.Date == currentCreationTime.Date && b.TransactionType == currentTransactionType && b.ProjectId == project.Id).Sum(a => a.Amount) / 1000000
                        });
                    }
                    rows.Add(append);
                }

            }

            model.Rows = rows.OrderBy(a => a.DateTime).ThenBy(a => a.TransactionType).ToList();




            var billRows = new List<ReceiveBillsRowViewModel>();

            foreach (var item in receiveBills)
            {
                var currentYear = new PersianDateTime(item.ReceiveDate).Year;

                var append = new ReceiveBillsRowViewModel();
                append.Year = currentYear;

                if (!billRows.Any(a => a.Year == currentYear))
                {
                    foreach (var project in projects)
                    {
                        append.ProjectBills.Add(new ReceiveBillsColumnViewModel()
                        {
                            ProjectId = project.Id,
                            ProjectName = project.Title,
                            ShowInTable = project.ShowInUsageReport,
                            DisplayOrder = project.UsageReportDisplayOrder,
                            YearTotalAmount = receiveBills.Where(b => new PersianDateTime(item.ReceiveDate).Year == currentYear && b.ProjectId == project.Id).Sum(a => a.Amount) / 1000000
                        });
                    }
                    billRows.Add(append);
                }
            }

            model.ReceiveBills = billRows.OrderBy(a => a.Year).ToList();




            var paymentRows = new List<PaymentsRowViewModel>();
            foreach (var item in payments)
            {
                var currentYear = new PersianDateTime(item.CreationTime).Year;

                var append = new PaymentsRowViewModel();
                append.Year = currentYear;

                if (!paymentRows.Any(a => a.Year == currentYear))
                {
                    foreach (var project in projects)
                    {
                        append.ProjectPayments.Add(new PaymentsColumnViewModel()
                        {
                            ProjectId = project.Id,
                            ProjectName = project.Title,
                            ShowInTable = project.ShowInUsageReport,
                            DisplayOrder = project.UsageReportDisplayOrder,
                            YearTotalAmount = payments.Where(b => new PersianDateTime(item.CreationTime).Year == currentYear && b.ProjectId == project.Id).Sum(a => a.Amount) / 1000000
                        });
                    }
                    paymentRows.Add(append);
                }
            }

            model.Payments = paymentRows.OrderBy(a => a.Year).ToList();






            var cashRemainignRows = new List<CashRemainingRowViewModel>();

            foreach (var item in receiveBills)
            {
                var currentYear = new PersianDateTime(item.ReceiveDate).Year;

                var append = new CashRemainingRowViewModel();
                append.Year = currentYear;

                if (!cashRemainignRows.Any(a => a.Year == currentYear))
                {
                    foreach (var project in projects)
                    {
                        var incomes = transactions.Where(b => new PersianDateTime(item.ReceiveDate).Year == currentYear && b.ProjectId == project.Id && b.TransactionType == TransactionType.Receive).Sum(a => a.Amount);
                        var costs = transactions.Where(b => new PersianDateTime(item.ReceiveDate).Year == currentYear && b.ProjectId == project.Id && b.TransactionType == TransactionType.Payment).Sum(a => a.Amount);

                        append.ProjectCashRemainings.Add(new CashRemainingColumnViewModel()
                        {
                            ProjectId = project.Id,
                            ProjectName = project.Title,
                            ShowInTable = project.ShowInUsageReport,
                            DisplayOrder = project.UsageReportDisplayOrder,
                            YearTotalAmount = (incomes - costs) / 1000000
                        });
                    }
                    cashRemainignRows.Add(append);
                }
            }

            model.CashReaminings = cashRemainignRows.OrderBy(a => a.Year).ToList();



            return View(model);
        }


    }
}