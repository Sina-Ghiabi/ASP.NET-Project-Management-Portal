using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Nexora.PMPortal.Controllers;
using Nexora.PMPortal.Projects;
using Nexora.PMPortal.Financial;
using System.Threading.Tasks;
using Nexora.PMPortal.Web.ViewModels;
using System.Linq;
using Abp.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nexora.PMPortal.Enums;
using MD.PersianDateTime;
using System;
using System.Collections.Generic;
using Nexora.PMPortal.Web.ViewModels.ChartForm;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Abp.Extensions;
using System.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Nexora.PMPortal.ProjectsDocumentations;

namespace Nexora.PMPortal.Web.Controllers
{
    [AbpMvcAuthorize]
    public class DashboardController : PMPortalControllerBase
    {
        private readonly IPaymentRequestAppService _paymentRequestAppService;
        private readonly IReceiveBillAppService _receiveBillAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IProjectAppService _projectAppService;
        private readonly IContractorAppService _contractorAppService;
        private readonly IProject_User_MappingAppService _project_User_MappingAppService;
        private readonly ICostBenefitAppService _costBenefitAppService;
        private readonly ILaborInventoryAppService _laborInventoryAppService;
        private readonly IReceiveAndPaymentAppService _receiveAndPaymentAppService;
        private readonly IWarrantyBalanceAppService _warrantyBalanceAppService;
        private readonly IProgressPercentageAppService _progressPercentageAppService;
        private readonly IGrossProfitToNetAppService _grossProfitToNetAppService;
        private readonly IProjectsTasksAppService _projectsTasksAppService;
        private readonly IProjectsTasksRevisionsAppService _projectsTasksRevisionsAppService;

        public decimal itemTotal { get; private set; }

        public DashboardController(IPaymentRequestAppService paymentRequestAppService,
            IReceiveBillAppService receiveBillAppService,
            ITransactionAppService transactionAppService,
            IProjectAppService projectAppService,
            IContractorAppService contractorAppService,
            IProject_User_MappingAppService project_User_MappingAppService,
            ICostBenefitAppService costBenefitAppService,
            ILaborInventoryAppService laborInventoryAppService,
            IReceiveAndPaymentAppService receiveAndPaymentAppService,
            IWarrantyBalanceAppService warrantyBalanceAppService,
            IProgressPercentageAppService progressPercentageAppService,
            IGrossProfitToNetAppService grossProfitToNetAppService,
            IProjectsTasksAppService projectsTasksAppService,
            IProjectsTasksRevisionsAppService projectsTasksRevisionsAppService
            )
        {
            _paymentRequestAppService = paymentRequestAppService;
            _receiveBillAppService = receiveBillAppService;
            _transactionAppService = transactionAppService;
            _projectAppService = projectAppService;
            _contractorAppService = contractorAppService;
            _project_User_MappingAppService = project_User_MappingAppService;
            _costBenefitAppService = costBenefitAppService;
            _laborInventoryAppService = laborInventoryAppService;
            _receiveAndPaymentAppService = receiveAndPaymentAppService;
            _warrantyBalanceAppService = warrantyBalanceAppService;
            _progressPercentageAppService = progressPercentageAppService;
            _grossProfitToNetAppService = grossProfitToNetAppService;
            _projectsTasksAppService = projectsTasksAppService;
            _projectsTasksRevisionsAppService = projectsTasksRevisionsAppService;
        }

        public async Task<ActionResult> Projects(int? id)
        {
            var userId = AbpSession.UserId.Value;

            var items = await _project_User_MappingAppService.GetUserProjects(userId);

            var records = items.Select(a => new ProjectViewModel()
            {
                Id = a.ProjectId,
                Title = a.Project.Title
            }).ToList();

            ViewBag.ProjectId = new SelectList(records, "Id", "Title", id);

            return View();
        }

        public async Task<ActionResult> PaymentAndReceive(int id)
        {
            var items = await _receiveAndPaymentAppService.GetProjectItems(id);
            ViewBag.Id = id;
            return View(items);
        }




        [DontWrapResult]
        public async Task<ActionResult> GetCostBenefitChartDatas(int projectId)
        {
            var userId = AbpSession.UserId.Value;

            var projects = await _project_User_MappingAppService.GetUserProjects(userId);

            var costbenefits = await _costBenefitAppService.GetProjectCostBenefits(projectId);

            var items = new List<CostBenefitSaleChartViewModel>();

            for (int i = 0; i < costbenefits.Count; i++)
            {
                var LastMonthValue = "";
                if (costbenefits[i].CostBenefitPeriod == CostBenefitPeriod.Three_Month)
                {
                    LastMonthValue = CostBenefitPeriod.Three_Month.GetDisplayName();
                }
                else if (costbenefits[i].CostBenefitPeriod == CostBenefitPeriod.Six_Month)
                {
                    LastMonthValue = CostBenefitPeriod.Six_Month.GetDisplayName();
                }
                else if (costbenefits[i].CostBenefitPeriod == CostBenefitPeriod.Nine_Month)
                {
                    LastMonthValue = CostBenefitPeriod.Nine_Month.GetDisplayName();
                }

                var record = new CostBenefitSaleChartViewModel
                {
                    Index = i,
                    Year = new PersianDateTime(costbenefits[i].Year).Year,
                    LastMonth = LastMonthValue,
                    GrossProfit = costbenefits[i].GrossProfit,
                    //YearSale = costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales,
                    //TotalSales = i == 0 ? costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales : items.FirstOrDefault(a => a.Index == i - 1).TotalSales + costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales,
                    //YearProfit = costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales - costbenefits[i].AllCost,
                    //TotalProfits = i == 0 ? costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales - costbenefits[i].AllCost : costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales - costbenefits[i].AllCost + items.FirstOrDefault(a => a.Index == i - 1).YearProfit
                };
                items.Add(record);
            }

            var years = items.Select(a => new CostBenefitChartViewModel
            {
                Year = a.Year,
                LastMonth = a.LastMonth,
                Amount = a.GrossProfit
            }).OrderBy(b => b.Year).ToList();

            return Json(new
            {
                years = years.Select(a => a.Year),
                lastmonth = years.Select(a => a.LastMonth),
                profits = years.Select(a => Math.Ceiling(a.Amount))
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> GetCostBenefitSaleChartDatas(int projectId)
        {
            var userId = AbpSession.UserId.Value;

            var projects = await _project_User_MappingAppService.GetUserProjects(userId);

            var costbenefits = (await _costBenefitAppService.GetProjectCostBenefits(projectId)).OrderBy(a => new PersianDateTime(a.Year).Year).ToList();

            var items = new List<CostBenefitSaleChartViewModel>();

            for (int i = 0; i < costbenefits.Count; i++)
            {
                var LastMonthValue = "";
                if (costbenefits[i].CostBenefitPeriod == CostBenefitPeriod.Three_Month)
                {
                    LastMonthValue = CostBenefitPeriod.Three_Month.GetDisplayName();
                }
                else if (costbenefits[i].CostBenefitPeriod == CostBenefitPeriod.Six_Month)
                {
                    LastMonthValue = CostBenefitPeriod.Six_Month.GetDisplayName();
                }
                else if (costbenefits[i].CostBenefitPeriod == CostBenefitPeriod.Nine_Month)
                {
                    LastMonthValue = CostBenefitPeriod.Nine_Month.GetDisplayName();
                }

                var record = new CostBenefitSaleChartViewModel
                {
                    Index = i,
                    Year = new PersianDateTime(costbenefits[i].Year).Year,
                    LastMonth = LastMonthValue,
                    TotalSales = i == 0 ? costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales : items.FirstOrDefault(a => a.Index == i - 1).TotalSales + costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales
                };

                items.Add(record);
            }


            var amalkardis = new List<CostBenefitSaleChartViewModel>();

            for (int i = 0; i < costbenefits.Count; i++)
            {
                var record = new CostBenefitSaleChartViewModel
                {
                    Index = i,
                    Year = new PersianDateTime(costbenefits[i].Year).Year,
                    TotalSales = i == 0 ? costbenefits[i].AllCost : amalkardis.FirstOrDefault(a => a.Index == i - 1).TotalSales + costbenefits[i].AllCost
                };
                amalkardis.Add(record);
            }

            var approvedSales = costbenefits.Select(a => new CostBenefitChartViewModel
            {
                Year = new PersianDateTime(a.Year).Year,
                Amount = a.ApprovedSales
            }).OrderBy(b => b.Year).ToList();

            var recycleSales = costbenefits.Select(a => new CostBenefitChartViewModel
            {
                Year = new PersianDateTime(a.Year).Year,
                Amount = a.RecycleSales
            }).OrderBy(b => b.Year).ToList();


            //var amalkardiSales = costbenefits.Select(a => new CostBenefitChartViewModel
            //{
            //    Year = new PersianDateTime(a.Year).Year,
            //    Amount = a.AllCost
            //}).OrderBy(b => b.Year).ToList();

            return Json(new
            {
                years = items.Select(a => a.Year),
                lastmonth = items.Select(a => a.LastMonth),
                approvedSales = approvedSales.Select(a => Math.Ceiling(a.Amount / 1000000)),
                recycleSales = recycleSales.Select(a => Math.Ceiling(a.Amount / 1000000)),
                amalkardiSales = amalkardis.Select(a => Math.Round(a.TotalSales / 1000000)),
                totalSales = items.Select(a => Math.Round(a.TotalSales / 1000000))

            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }






        [DontWrapResult]
        public async Task<ActionResult> GetProgressPercentageChartDatas(int projectId)
        {
            var userId = AbpSession.UserId.Value;

            var projects = await _project_User_MappingAppService.GetUserProjects(userId);

            var progressPercentages = (await _progressPercentageAppService.GetProjectProgressPercentages(projectId)).OrderBy(a => a.Year).ToList();

            //var items = new List<ProgressPercentageChartViewModel>();

            //for (int i = 0; i < progressPercentages.Count; i++)
            //{
            //    var record = new ProgressPercentageChartViewModel
            //    {
            //        Index = i,
            //        Year = (int)progressPercentages[i].Year,
            //        //Total = i == 0 ? progressPercentages[i].TotalProgressReal :
            //        //                      items.FirstOrDefault(a => a.Index == i - 1).Total + progressPercentages[i].TotalProgressReal
            //        Total = progressPercentages[i].TotalProgressReal
            //    };
            //    items.Add(record);
            //}

            var records = progressPercentages.GroupBy(a => a.Year).Select(g => new
            {
                Key = g.Key,
                Value = g.Sum(s => s.TotalProgressReal),
                Year = g.First().Year
            }).ToList();


            var items = new List<ProgressPercentageChartViewModel>();

            for (int i = 0; i < records.Count; i++)
            {
                var record = new ProgressPercentageChartViewModel
                {
                    Index = i,
                    Year = (int)records[i].Year,
                    Total = i == 0 ? records[i].Value : items.FirstOrDefault(a => a.Index == i - 1).Total + records[i].Value
                };
                items.Add(record);
            }



            //var yearlyProgressPercentage = progressPercentages.Select(a => new CostBenefitChartViewModel
            //{
            //    Year = (int)a.Year,
            //    Amount = a.EngineeringProgressReal + a.ExecutionProgressReal + a.SupplyProgressReal + a.TotalProgressReal
            //}).OrderBy(b => b.Year).ToList();

            return Json(new
            {
                years = records.Select(a => a.Year),
                totalProgressPercentage = items.Select(a => a.Total),
                yearlyProgressPercentage = records.Select(a => a.Value)
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }



        [DontWrapResult]
        public async Task<ActionResult> GetlaborInventoryChartDatas(int projectId)
        {
            var labors = await _laborInventoryAppService.GetProjectLaborInventories(projectId);

            var items = labors.Select(a => new CostBenefitChartViewModel
            {
                Year = new PersianDateTime(a.Year).Year,

                LastMonth = a.Month12_Amount > 0 ? "Esfand" :
                         a.Month11_Amount > 0 ? "Bahman" :
                         a.Month10_Amount > 0 ? "Dey" :
                         a.Month9_Amount > 0 ? "Azar" :
                         a.Month8_Amount > 0 ? "Aban" :
                         a.Month7_Amount > 0 ? "Mehr" :
                         a.Month6_Amount > 0 ? "Shahrivar" :
                         a.Month5_Amount > 0 ? "Mordad" :
                         a.Month4_Amount > 0 ? "Tir" :
                         a.Month3_Amount > 0 ? "Khordad" :
                         a.Month2_Amount > 0 ? "Ordibehesht" :
                         a.Month1_Amount > 0 ? "Farvardin" : "",

                Amount = a.Month12_Amount > 0 ? a.Month12_Amount :
                         a.Month11_Amount > 0 ? a.Month11_Amount :
                         a.Month10_Amount > 0 ? a.Month10_Amount :
                         a.Month9_Amount > 0 ? a.Month9_Amount :
                         a.Month8_Amount > 0 ? a.Month8_Amount :
                         a.Month7_Amount > 0 ? a.Month7_Amount :
                         a.Month6_Amount > 0 ? a.Month6_Amount :
                         a.Month5_Amount > 0 ? a.Month5_Amount :
                         a.Month4_Amount > 0 ? a.Month4_Amount :
                         a.Month3_Amount > 0 ? a.Month3_Amount :
                         a.Month2_Amount > 0 ? a.Month2_Amount :
                         a.Month1_Amount > 0 ? a.Month1_Amount : 0
            }).OrderBy(b => b.Year).ToList();

            return Json(new
            {
                years = items.Select(a => a.Year),
                lastmonth = items.Select(a => a.LastMonth),
                labors = items.Select(a => Math.Ceiling(a.Amount))
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> GetReceiveAndPaymentChartDatas(int projectId)
        {
            var userId = AbpSession.UserId.Value;

            var projects = await _project_User_MappingAppService.GetUserProjects(userId);

            var receiveAndPayments = (await _receiveAndPaymentAppService.GetProjectItems(projectId)).ToList();

            var receives_index = 0;
            var receives = receiveAndPayments.Select(a => new ReceiveAndPaymentChartViewModel
            {
                Index = receives_index++,
                Year = new PersianDateTime(a.Year).Year,
                Amount = a.PreReceived + a.Statements + a.DepositRelease + a.WarrantyReduce + a.OtherReceipt + a.IndirectReceipt + a.OnAccountReceipt
            }).OrderBy(b => b.Year).ToList();

            var payments_index = 0;
            var payments = receiveAndPayments.Select(a => new ReceiveAndPaymentChartViewModel
            {
                Index = payments_index++,
                Year = new PersianDateTime(a.Year).Year,
                Amount = a.ProjectPayments + a.WarrantyWithWage + a.SaleryWithTax + a.IndirecetPayment + a.ValueAddedTax
            }).OrderBy(b => b.Year).ToList();

            var receivesTajamoyi = new List<ReceiveAndPaymentChartViewModel>();

            for (int i = 0; i < receiveAndPayments.Count; i++)
            {
                var record = new ReceiveAndPaymentChartViewModel
                {
                    Index = i,
                    Year = (new PersianDateTime(receiveAndPayments[i].Year)).Year,
                    Amount = i == 0 ? receives[i].Amount : receivesTajamoyi.FirstOrDefault(b => b.Index == i - 1).Amount + receives[i].Amount,
                };
                receivesTajamoyi.Add(record);
            }


            var paymentsTajamoyi = new List<ReceiveAndPaymentChartViewModel>();

            for (int i = 0; i < receiveAndPayments.Count; i++)
            {
                var record = new ReceiveAndPaymentChartViewModel
                {
                    Index = i,
                    Year = (new PersianDateTime(receiveAndPayments[i].Year)).Year,
                    Amount = i == 0 ? payments[i].Amount : paymentsTajamoyi.FirstOrDefault(b => b.Index == i - 1).Amount + payments[i].Amount,
                };
                paymentsTajamoyi.Add(record);
            }

            return Json(new
            {
                years = receives.Select(a => a.Year),
                receives = receives.Select(a => a.Amount / 1000000),
                payments = payments.Select(a => a.Amount / 1000000),
                receivesTajamoyi = receivesTajamoyi.Select(a => a.Amount / 1000000),
                paymentsTajamoyi = paymentsTajamoyi.Select(a => a.Amount / 1000000)
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [DontWrapResult]
        public async Task<ActionResult> GetWarrantyBalanceChartDatas(int projectId)
        {
            var userId = AbpSession.UserId.Value;

            var projects = await _project_User_MappingAppService.GetUserProjects(userId);

            var warrantyBalances = await _warrantyBalanceAppService.GetProjectWarrantyBalances(projectId);

            var pishDaryaft = warrantyBalances.Select(a => new CostBenefitChartViewModel
            {
                Year = new PersianDateTime(a.Year).Year,
                Amount = a.PreReceived
            }).OrderBy(b => b.Year).ToList();

            var ejrayeTahodat = warrantyBalances.Select(a => new CostBenefitChartViewModel
            {
                Year = new PersianDateTime(a.Year).Year,
                Amount = a.ObligationExecution
            }).OrderBy(b => b.Year).ToList();

            var vajhoZeman = warrantyBalances.Select(a => new CostBenefitChartViewModel
            {
                Year = new PersianDateTime(a.Year).Year,
                Amount = a.GuaranteeDeduction
            }).OrderBy(b => b.Year).ToList();

            return Json(new
            {
                years = pishDaryaft.Select(a => a.Year),
                pishDaryaft = pishDaryaft.Select(a => a.Amount / 1000000),
                ejrayeTahodat = ejrayeTahodat.Select(a => a.Amount / 1000000),
                vajhoZeman = vajhoZeman.Select(a => a.Amount / 1000000)
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }



        [DontWrapResult]
        public async Task<ActionResult> GetlaborInventoryMonthlyChartDatas(int projectId, int year)
        {
            var labors = await _laborInventoryAppService.GetProjectLaborInventories(projectId);
            var laborsInYear = labors.FirstOrDefault(a => new PersianDateTime(a.Year).Year == year);

            var list = new List<CostBenefitMonthlyChartViewModel>();

            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)1),
                Amount = laborsInYear.Month1_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)2),
                Amount = laborsInYear.Month2_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)3),
                Amount = laborsInYear.Month3_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)4),
                Amount = laborsInYear.Month4_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)5),
                Amount = laborsInYear.Month5_Amount
            });

            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)6),
                Amount = laborsInYear.Month6_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)7),
                Amount = laborsInYear.Month7_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)8),
                Amount = laborsInYear.Month8_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)9),
                Amount = laborsInYear.Month9_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)10),
                Amount = laborsInYear.Month10_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)11),
                Amount = laborsInYear.Month11_Amount
            });
            list.Add(new CostBenefitMonthlyChartViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)12),
                Amount = laborsInYear.Month12_Amount
            });

            return Json(new
            {
                month = list.Select(a => a.Month),
                labors = list.Select(a => Math.Ceiling(a.Amount))
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> GetProgressPercentageMonthlyChartDatas(int projectId, int year)
        {
            var userId = AbpSession.UserId.Value;

            var projects = await _project_User_MappingAppService.GetUserProjects(userId);

            var items = (await _progressPercentageAppService.GetProjectProgressPercentages(projectId)).ToList();

            var yearItems = items.Where(a => (int)a.Year == year).ToList();

            var months = new List<ProgressPercentageMonthlyChartViewModel>();

            for (int i = 1; i < 13; i++)
            {
                var record = yearItems.FirstOrDefault(a => (int)a.Month == i);
                var item = new ProgressPercentageMonthlyChartViewModel();
                item.Index = i;
                item.Month = i;
                item.MonthName = EnumExtensions.GetDisplayName((SelectiveMonth)i);
                if (record != null)
                {
                    //item.TotalPlan = record.EngineeringProgressPlan + record.ExecutionProgressPlan + record.SupplyProgressPlan + record.TotalProgressPlan;
                    //item.TotalReal = record.EngineeringProgressReal + record.ExecutionProgressReal + record.SupplyProgressReal + record.TotalProgressReal;
                    item.TotalPlan = record.TotalProgressPlan;
                    item.TotalReal = record.TotalProgressReal;
                }
                else
                {
                    item.TotalPlan = 0;
                    item.TotalReal = 0;
                }


                months.Add(item);
            };

            return Json(new
            {
                months = months.Select(a => a.MonthName),
                plans = months.Select(a => a.TotalPlan),
                reals = months.Select(a => a.TotalReal),
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> GetProgressPercentageMonthChartDatas(int projectId, int year, int month)
        {
            var userId = AbpSession.UserId.Value;

            var projects = await _project_User_MappingAppService.GetUserProjects(userId);

            var receiveAndPayments = (await _progressPercentageAppService.GetProjectProgressPercentages(projectId)).ToList();

            var yearReceiveAndPayments = receiveAndPayments.Where(a => (int)a.Year == year).ToList();

            var months = new List<ProgressPercentageMonthlyChartViewModel>();

            var record = yearReceiveAndPayments.FirstOrDefault(a => (int)a.Month == month);
            var currentMonth = (SelectiveMonth)month;

            return Json(new
            {
                plans_Engineering = record != null ? record.EngineeringProgressPlan : 0,
                reals_Engineering = record != null ? record.EngineeringProgressReal : 0,

                plans_Execution = record != null ? record.ExecutionProgressPlan : 0,
                reals_Execution = record != null ? record.ExecutionProgressReal : 0,

                plans_Supply = record != null ? record.SupplyProgressPlan : 0,
                reals_Supply = record != null ? record.SupplyProgressReal : 0,
                monthName = currentMonth.GetDisplayName()
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }





        public async Task<ActionResult> Managers()
        {
            var model = new ManagerDashboardViewModel();
            var i = new PersianDateTime(DateTime.Now.AddYears(-5)).Year;
            model.DefaultYear = (SelectiveYear)i;

            return View(model);
        }


        [DontWrapResult]
        public async Task<ActionResult> GetforooshByTypeChartData(int year)
        {
            var userId = AbpSession.UserId.Value;

            var costbenefits = (await _costBenefitAppService.GetYearCostBenefits(year)).OrderBy(a => new PersianDateTime(a.Year).Year).ToList();

            var reportItems = costbenefits.GroupBy(a => a.Year).Select(g => new
            {
                Key = g.Key,
                LastMonth = g.Select(s => s.CostBenefitPeriod.GetDisplayName()),
                Value = g.Sum(s => s.ApprovedSales + s.RecycleSales),
                ApprovedSales = g.Sum(s => s.ApprovedSales),
                RecycleSales = g.Sum(s => s.RecycleSales),
                Year = new PersianDateTime(g.First().Year).Year
            });

            var approvedSales = reportItems.Select(a => new CostBenefitChartViewModel
            {
                Year = a.Year,
                Amount = a.ApprovedSales
            }).OrderBy(b => b.Year).ToList();

            var recycleSales = reportItems.Select(a => new CostBenefitChartViewModel
            {
                Year = a.Year,
                Amount = a.RecycleSales
            }).OrderBy(b => b.Year).ToList();

            return Json(new
            {
                years = reportItems.Select(a => a.Year),
                lastmonth = reportItems.Select(a=>a.LastMonth),
                approvedSales = reportItems.Select(a => Math.Ceiling(a.ApprovedSales) / 1000000),
                recycleSales = reportItems.Select(a => Math.Ceiling(a.RecycleSales) / 1000000),
                totalSales = reportItems.Select(a => Math.Ceiling(a.Value) / 1000000)

            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [DontWrapResult]
        public async Task<ActionResult> GetforooshByTypeYearlyChartData(int year)
        {
            var userId = AbpSession.UserId.Value;
            var random = new Random();

            var costbenefits = (await _costBenefitAppService.GetSpecificYearCostBenefits(year)).OrderBy(a => new PersianDateTime(a.Year).Year).ToList();

            var reportItems = costbenefits.GroupBy(a => a.ProjectId).Select(g => new
            {
                Key = g.First().Project.Title,
                Value = g.Sum(s => s.ApprovedSales + s.RecycleSales),
                Color = String.Format("#{0:X6}", random.Next(0x1000000))
            }).OrderByDescending(a => a.Value).ToList();

            var totalInYear = costbenefits.Sum(a => a.ApprovedSales + a.RecycleSales);


            return Json(new
            {
                projects = reportItems.Select(a => a.Key),
                colors = reportItems.Select(a => a.Color),
                yearlySales = reportItems.Select(a => Math.Round(a.Value / totalInYear, 2) * 100)
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> GetforooshByServiceTypeChartData(int year)
        {
            var userId = AbpSession.UserId.Value;

            var costbenefits = (await _costBenefitAppService.GetYearCostBenefits(year)).OrderBy(a => new PersianDateTime(a.Year).Year).ToList();

            List<int> Allyears = new List<int>(new int[] { 1388, 1389, 1390, 1391, 1392, 1393, 1394, 1395, 1396, 1397, 1398, 1399, 1400, 1401 });
            var result = new List<CostBenefitByTypeViewModel>();

            for (int i = year; i <= 1401; i++)
            {
                var item = new CostBenefitByTypeViewModel();
                item.Year = Allyears.FirstOrDefault(a => a == i);

                item.LastMonth = string.Join(",", costbenefits.Where(a => new PersianDateTime(a.Year).Year == i).Select(a=>a.CostBenefitPeriod.GetDisplayName()));

                item.Total = costbenefits.Where(a => new PersianDateTime(a.Year).Year == i).Sum(a => a.ApprovedSales + a.RecycleSales);

                item.WaterAndWasteWater = costbenefits.Where(
                    a => new PersianDateTime(a.Year).Year == i &&
                    a.ServiceType == CostBenefitServiceType.WaterAndWasteWater)
                .Sum(a => a.ApprovedSales + a.RecycleSales);

                item.ElectricityAndEnergy = costbenefits.Where(a => new PersianDateTime(a.Year).Year == i && a.ServiceType == CostBenefitServiceType.ElectricityAndEnergy)
                .Sum(a => a.ApprovedSales + a.RecycleSales);

                item.Utilty = costbenefits.Where(a => new PersianDateTime(a.Year).Year == i && a.ServiceType == CostBenefitServiceType.Utilty)
                .Sum(a => a.ApprovedSales + a.RecycleSales);

                item.WaterSupply = costbenefits.Where(a => new PersianDateTime(a.Year).Year == i && a.ServiceType == CostBenefitServiceType.WaterSupply)
                .Sum(a => a.ApprovedSales + a.RecycleSales);

                item.IndustrialDevelopment = costbenefits.Where(a => new PersianDateTime(a.Year).Year == i && a.ServiceType == CostBenefitServiceType.IndustrialDevelopment)
                .Sum(a => a.ApprovedSales + a.RecycleSales);

                if (item.Total > 0)
                {
                    result.Add(item);
                }
            }

            return Json(new
            {
                years = result.Select(a => a.Year),
                lastmonth = result.Select(a =>a.LastMonth),
                waterAndWasteWater = result.Select(a => Math.Ceiling(a.WaterAndWasteWater) / 1000000),
                electricityAndEnergy = result.Select(a => Math.Ceiling(a.ElectricityAndEnergy) / 1000000),
                utilty = result.Select(a => Math.Ceiling(a.Utilty) / 1000000),
                waterSupply = result.Select(a => Math.Ceiling(a.WaterSupply) / 1000000),
                industrialDevelopment = result.Select(a => Math.Ceiling(a.IndustrialDevelopment) / 1000000),
                totalSales = result.Select(a => Math.Ceiling(a.Total) / 1000000)
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [DontWrapResult]
        public async Task<ActionResult> GetforooshByTypeYearlyProjectsData(int year)
        {
            var userId = AbpSession.UserId.Value;

            var costbenefits = (await _costBenefitAppService.GetSpecificYearCostBenefits(year)).Where(a => (a.ApprovedSales + a.RecycleSales) > 0).ToList();

            var WaterAndWasteWater = string.Join("", costbenefits.Where(a => a.ServiceType == CostBenefitServiceType.WaterAndWasteWater).Select(a => "<div>" + a.Project.Title + "</div>").ToList());
            var WaterSupply = string.Join("", costbenefits.Where(a => a.ServiceType == CostBenefitServiceType.WaterSupply).Select(a => "<div>" + a.Project.Title + "</div>").ToList());
            var ElectricityAndEnergy = string.Join("", costbenefits.Where(a => a.ServiceType == CostBenefitServiceType.ElectricityAndEnergy).Select(a => "<div>" + a.Project.Title + "</div>").ToList());
            var IndustrialDevelopment = string.Join("", costbenefits.Where(a => a.ServiceType == CostBenefitServiceType.IndustrialDevelopment).Select(a => "<div>" + a.Project.Title + "</div>").ToList());
            var Utilty = string.Join("", costbenefits.Where(a => a.ServiceType == CostBenefitServiceType.Utilty).Select(a => "<div>" + a.Project.Title + "</div>").ToList());

            return Json(new
            {
                waterAndWasteWater = WaterAndWasteWater,
                waterSupply = WaterSupply,
                electricityAndEnergy = ElectricityAndEnergy,
                industrialDevelopment = IndustrialDevelopment,
                utilty = Utilty
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [DontWrapResult]
        public async Task<ActionResult> GetroshdForooshChartData(int year)
        {
            var userId = AbpSession.UserId.Value;

            var records = (await _costBenefitAppService.GetYearCostBenefits(1393)).OrderBy(a => new PersianDateTime(a.Year).Year).ToList();

            var costbenefits = records.GroupBy(a => a.Year).Select(g => new
            {
                Key = g.Key,
                Months = string.Join("," , g.Select(a=>a.CostBenefitPeriod.GetDisplayName())),
                Value = g.Sum(s => s.ApprovedSales + s.RecycleSales),
                ApprovedSales = g.Sum(s => s.ApprovedSales),
                RecycleSales = g.Sum(s => s.RecycleSales),
                Year = new PersianDateTime(g.First().Year).Year
            }).ToList();

            var items = new List<CostBenefitSaleChartViewModel>();

            for (int i = 0; i < costbenefits.Count; i++)
            {
                var record = new CostBenefitSaleChartViewModel
                {
                    Index = i,
                    Year = costbenefits[i].Year,
                    LastMonth = costbenefits[i].Months,
                    TotalSales = ((costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales) - (costbenefits[0].RecycleSales + costbenefits[0].ApprovedSales)) / (costbenefits[0].RecycleSales + costbenefits[0].ApprovedSales) * 100
                };
                items.Add(record);
            }

            return Json(new
            {
                years = items.Skip(1).Select(a => a.Year),
                lastmonth = items.Select(a=>a.LastMonth),
                totalSales = items.Skip(1).Select(a => Math.Ceiling(a.TotalSales))

            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> GetroshdForooshWithLastYearChartData(int year)
        {
            var userId = AbpSession.UserId.Value;

            var records = (await _costBenefitAppService.GetYearCostBenefits(year)).OrderBy(a => new PersianDateTime(a.Year).Year).ToList();


            var costbenefits = records.GroupBy(a => a.Year).Select(g => new
            {
                Key = g.Key,
                LastMonth = g.Select(a=>a.CostBenefitPeriod.GetDisplayName()),
                Value = g.Sum(s => s.ApprovedSales + s.RecycleSales),
                ApprovedSales = g.Sum(s => s.ApprovedSales),
                RecycleSales = g.Sum(s => s.RecycleSales),
                InflationRate = g.Min(s => s.InflationRate),
                Year = new PersianDateTime(g.First().Year).Year
            }).ToList();


            var items = new List<CostBenefitSaleChartViewModel>();

            for (int i = 0; i < costbenefits.Count; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                var record = new CostBenefitSaleChartViewModel();
                record.Index = i;
                record.Year = costbenefits[i].Year;
                record.LastMonth = string.Join(",",costbenefits[i].LastMonth);
                var currentYear = (costbenefits[i].RecycleSales + costbenefits[i].ApprovedSales);
                var lastYear = (costbenefits[i - 1].RecycleSales + costbenefits[i - 1].ApprovedSales);
                record.TotalSales = ((currentYear - lastYear) / lastYear) * 100;
                items.Add(record);
            }

            var tavarom = costbenefits.Skip(1).Select(a => new CostBenefitChartViewModel
            {
                Year = a.Year,
                Amount = a.InflationRate
            }).OrderBy(b => b.Year).ToList();


            return Json(new
            {
                years = items.Select(a => a.Year),
                lastmonth = items.Select(a=>a.LastMonth),
                totalSales = items.Select(a => Math.Ceiling(a.TotalSales)),
                tavarom = tavarom.Select(a => Math.Ceiling(a.Amount))

            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [DontWrapResult]
        public async Task<ActionResult> GethashieSoodNakhalesChartData(int year)
        {
            var userId = AbpSession.UserId.Value;

            var grossProfitToNets = (await _grossProfitToNetAppService.GetGrossProfitToNetsWithYear(year)).OrderBy(a => new PersianDateTime(a.Year).Year).ToList();

            var items = new List<GrossProfitToNetChartViewModel>();

            for (int i = 0; i < grossProfitToNets.Count; i++)
            {
                var record = new GrossProfitToNetChartViewModel
                {
                    Index = i,
                    Year = new PersianDateTime(grossProfitToNets[i].Year).Year,
                    Amount = grossProfitToNets[i].GrossProfit
                };
                items.Add(record);
            }
            var records = items.GroupBy(a => a.Year).Select(g => new
            {
                Key = g.Key,
                Value = g.Sum(s => s.Amount),
                Year = g.First().Year
            });

            return Json(new
            {
                years = records.Select(a => a.Year),
                values = records.Select(a => Math.Round(a.Value, 2))
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> GethashieSoodkhalesChartData(int year)
        {
            var userId = AbpSession.UserId.Value;

            var grossProfitToNets = (await _grossProfitToNetAppService.GetGrossProfitToNetsWithYear(year)).OrderBy(a => new PersianDateTime(a.Year).Year).ToList();

            var items = new List<GrossProfitToNetChartViewModel>();

            for (int i = 0; i < grossProfitToNets.Count; i++)
            {
                var record = new GrossProfitToNetChartViewModel
                {
                    Index = i,
                    Year = new PersianDateTime(grossProfitToNets[i].Year).Year,
                    Amount = grossProfitToNets[i].NetProfit
                };
                items.Add(record);
            }
            var records = items.GroupBy(a => a.Year).Select(g => new
            {
                Key = g.Key,
                Value = g.Sum(s => s.Amount),
                Year = g.First().Year
            });

            return Json(new
            {
                years = records.Select(a => a.Year),
                values = records.Select(a => Math.Round(a.Value, 2))
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }



        [DontWrapResult]
        public async Task<ActionResult> GetCompanyLaborInventoryBySalesChartDatas(int year)
        {
            var labors = await _laborInventoryAppService.GetCompanyLaborInventories(year);
            var yearTotalSales = await _costBenefitAppService.GetYearCostBenefits(year);


            var totalEmployees = labors.GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            {
                Key = g.Key,
                Value = g.FirstOrDefault(a => a.Month12_Amount > 0)?.Month12_Amount > 0 ? g.Sum(b => b.Month12_Amount) :
             g.FirstOrDefault(a => a.Month11_Amount > 0)?.Month11_Amount > 0 ? g.Sum(b => b.Month11_Amount) :
             g.FirstOrDefault(a => a.Month10_Amount > 0)?.Month10_Amount > 0 ? g.Sum(b => b.Month10_Amount) :
             g.FirstOrDefault(a => a.Month9_Amount > 0)?.Month9_Amount > 0 ? g.Sum(b => b.Month9_Amount) :
             g.FirstOrDefault(a => a.Month8_Amount > 0)?.Month8_Amount > 0 ? g.Sum(b => b.Month8_Amount) :
             g.FirstOrDefault(a => a.Month7_Amount > 0)?.Month7_Amount > 0 ? g.Sum(b => b.Month7_Amount) :
             g.FirstOrDefault(a => a.Month6_Amount > 0)?.Month6_Amount > 0 ? g.Sum(b => b.Month6_Amount) :
             g.FirstOrDefault(a => a.Month5_Amount > 0)?.Month5_Amount > 0 ? g.Sum(b => b.Month5_Amount) :
             g.FirstOrDefault(a => a.Month4_Amount > 0)?.Month4_Amount > 0 ? g.Sum(b => b.Month4_Amount) :
             g.FirstOrDefault(a => a.Month3_Amount > 0)?.Month3_Amount > 0 ? g.Sum(b => b.Month3_Amount) :
             g.FirstOrDefault(a => a.Month2_Amount > 0)?.Month2_Amount > 0 ? g.Sum(b => b.Month2_Amount) :
             g.FirstOrDefault(a => a.Month1_Amount > 0)?.Month1_Amount > 0 ? g.Sum(b => b.Month1_Amount) : 0,
                Year = new PersianDateTime(g.First().Year).Year
            }).ToList();


            var totalSales = yearTotalSales.GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            {
                Key = g.Key,
                Year = new PersianDateTime(g.First().Year).Year,
                TotalSale = g.Sum(a => a.ApprovedSales + a.RecycleSales) / 1000000
            }).ToList();


            var items = new List<GrossProfitToNetChartViewModel>();

            foreach (var item in totalSales)
            {
                var thisYearHrTotal = totalEmployees.FirstOrDefault(a => a.Year == item.Year)?.Value;
                if (thisYearHrTotal != null)
                {
                      var record = new GrossProfitToNetChartViewModel
                          {
                              Year = item.Year,
                              Amount = thisYearHrTotal.Value > 0 ? item.TotalSale / thisYearHrTotal.Value : 0
                          };
                     items.Add(record);
                }
           
            }

            //var salesVolume = yearTotaLabors.GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            //{
            //    Key = g.Key,
            //    Value = g.FirstOrDefault(a => a.Month12_Amount > 0)?.Month12_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month12_Amount > 0).Month12_Amount :
            //             g.FirstOrDefault(a => a.Month11_Amount > 0)?.Month11_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month11_Amount > 0).Month11_Amount :
            //             g.FirstOrDefault(a => a.Month10_Amount > 0)?.Month10_Amount > 0 ? yearTotalSales.Where(a => a.Year == g.First().Year).Sum(a => a.ApprovedSales / a.Recycl) / g.FirstOrDefault(a => a.Month10_Amount > 0).Month10_Amount :
            //             g.FirstOrDefault(a => a.Month9_Amount > 0)?.Month9_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month9_Amount > 0).Month9_Amount :
            //             g.FirstOrDefault(a => a.Month8_Amount > 0)?.Month8_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month8_Amount > 0).Month8_Amount :
            //             g.FirstOrDefault(a => a.Month7_Amount > 0)?.Month7_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month7_Amount > 0).Month7_Amount :
            //             g.FirstOrDefault(a => a.Month6_Amount > 0)?.Month6_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month6_Amount > 0).Month6_Amount :
            //             g.FirstOrDefault(a => a.Month5_Amount > 0)?.Month5_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month5_Amount > 0).Month5_Amount :
            //             g.FirstOrDefault(a => a.Month4_Amount > 0)?.Month4_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month4_Amount > 0).Month4_Amount :
            //             g.FirstOrDefault(a => a.Month3_Amount > 0)?.Month3_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month3_Amount > 0).Month3_Amount :
            //             g.FirstOrDefault(a => a.Month2_Amount > 0)?.Month2_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month2_Amount > 0).Month2_Amount :
            //             g.FirstOrDefault(a => a.Month1_Amount > 0)?.Month1_Amount > 0 ? totalSales.FirstOrDefault(a => a.Year == new PersianDateTime(g.First().Year).Year).TotalSale / g.FirstOrDefault(a => a.Month1_Amount > 0).Month1_Amount : 0,
            //    Year = new PersianDateTime(g.First().Year).Year
            //}).ToList();

            return Json(new
            {
                years = items.Select(a => a.Year),

                salesVolume = items.Select(a => Math.Round(a.Amount)),
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }



        [DontWrapResult]
        public async Task<ActionResult> GetCompanyLaborInventoryChartDatas(int year)
        {
            var labors = await _laborInventoryAppService.GetCompanyLaborInventories(year);


            var totalEmployees = labors.GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            {
                Key = g.Key,
                Value = g.FirstOrDefault(a => a.Month12_Amount > 0)?.Month12_Amount > 0 ? g.Sum(b => b.Month12_Amount) :
                         g.FirstOrDefault(a => a.Month11_Amount > 0)?.Month11_Amount > 0 ? g.Sum(b => b.Month11_Amount) :
                         g.FirstOrDefault(a => a.Month10_Amount > 0)?.Month10_Amount > 0 ? g.Sum(b => b.Month10_Amount) :
                         g.FirstOrDefault(a => a.Month9_Amount > 0)?.Month9_Amount > 0 ? g.Sum(b => b.Month9_Amount) :
                         g.FirstOrDefault(a => a.Month8_Amount > 0)?.Month8_Amount > 0 ? g.Sum(b => b.Month8_Amount) :
                         g.FirstOrDefault(a => a.Month7_Amount > 0)?.Month7_Amount > 0 ? g.Sum(b => b.Month7_Amount) :
                         g.FirstOrDefault(a => a.Month6_Amount > 0)?.Month6_Amount > 0 ? g.Sum(b => b.Month6_Amount) :
                         g.FirstOrDefault(a => a.Month5_Amount > 0)?.Month5_Amount > 0 ? g.Sum(b => b.Month5_Amount) :
                         g.FirstOrDefault(a => a.Month4_Amount > 0)?.Month4_Amount > 0 ? g.Sum(b => b.Month4_Amount) :
                         g.FirstOrDefault(a => a.Month3_Amount > 0)?.Month3_Amount > 0 ? g.Sum(b => b.Month3_Amount) :
                         g.FirstOrDefault(a => a.Month2_Amount > 0)?.Month2_Amount > 0 ? g.Sum(b => b.Month2_Amount) :
                         g.FirstOrDefault(a => a.Month1_Amount > 0)?.Month1_Amount > 0 ? g.Sum(b => b.Month1_Amount) : 0,
                Year = new PersianDateTime(g.First().Year).Year
            }).ToList();


            var centralOfficeEmployees = labors.Where(a => a.ProjectId == 75).GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            {
                Key = g.Key,
                Value = g.FirstOrDefault(a => a.Month12_Amount > 0)?.Month12_Amount > 0 ? g.Sum(b => b.Month12_Amount) :
                         g.FirstOrDefault(a => a.Month11_Amount > 0)?.Month11_Amount > 0 ? g.Sum(b => b.Month11_Amount) :
                         g.FirstOrDefault(a => a.Month10_Amount > 0)?.Month10_Amount > 0 ? g.Sum(b => b.Month10_Amount) :
                         g.FirstOrDefault(a => a.Month9_Amount > 0)?.Month9_Amount > 0 ? g.Sum(b => b.Month9_Amount) :
                         g.FirstOrDefault(a => a.Month8_Amount > 0)?.Month8_Amount > 0 ? g.Sum(b => b.Month8_Amount) :
                         g.FirstOrDefault(a => a.Month7_Amount > 0)?.Month7_Amount > 0 ? g.Sum(b => b.Month7_Amount) :
                         g.FirstOrDefault(a => a.Month6_Amount > 0)?.Month6_Amount > 0 ? g.Sum(b => b.Month6_Amount) :
                         g.FirstOrDefault(a => a.Month5_Amount > 0)?.Month5_Amount > 0 ? g.Sum(b => b.Month5_Amount) :
                         g.FirstOrDefault(a => a.Month4_Amount > 0)?.Month4_Amount > 0 ? g.Sum(b => b.Month4_Amount) :
                         g.FirstOrDefault(a => a.Month3_Amount > 0)?.Month3_Amount > 0 ? g.Sum(b => b.Month3_Amount) :
                         g.FirstOrDefault(a => a.Month2_Amount > 0)?.Month2_Amount > 0 ? g.Sum(b => b.Month2_Amount) :
                         g.FirstOrDefault(a => a.Month1_Amount > 0)?.Month1_Amount > 0 ? g.Sum(b => b.Month1_Amount) : 0,
                Year = new PersianDateTime(g.First().Year).Year
            }).ToList();

            var projectEmployees = labors.Where(a => a.ProjectId != 75).GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            {
                Key = g.Key,
                Value = g.FirstOrDefault(a => a.Month12_Amount > 0)?.Month12_Amount > 0 ? g.Sum(b => b.Month12_Amount) :
                         g.FirstOrDefault(a => a.Month11_Amount > 0)?.Month11_Amount > 0 ? g.Sum(b => b.Month11_Amount) :
                         g.FirstOrDefault(a => a.Month10_Amount > 0)?.Month10_Amount > 0 ? g.Sum(b => b.Month10_Amount) :
                         g.FirstOrDefault(a => a.Month9_Amount > 0)?.Month9_Amount > 0 ? g.Sum(b => b.Month9_Amount) :
                         g.FirstOrDefault(a => a.Month8_Amount > 0)?.Month8_Amount > 0 ? g.Sum(b => b.Month8_Amount) :
                         g.FirstOrDefault(a => a.Month7_Amount > 0)?.Month7_Amount > 0 ? g.Sum(b => b.Month7_Amount) :
                         g.FirstOrDefault(a => a.Month6_Amount > 0)?.Month6_Amount > 0 ? g.Sum(b => b.Month6_Amount) :
                         g.FirstOrDefault(a => a.Month5_Amount > 0)?.Month5_Amount > 0 ? g.Sum(b => b.Month5_Amount) :
                         g.FirstOrDefault(a => a.Month4_Amount > 0)?.Month4_Amount > 0 ? g.Sum(b => b.Month4_Amount) :
                         g.FirstOrDefault(a => a.Month3_Amount > 0)?.Month3_Amount > 0 ? g.Sum(b => b.Month3_Amount) :
                         g.FirstOrDefault(a => a.Month2_Amount > 0)?.Month2_Amount > 0 ? g.Sum(b => b.Month2_Amount) :
                         g.FirstOrDefault(a => a.Month1_Amount > 0)?.Month1_Amount > 0 ? g.Sum(b => b.Month1_Amount) : 0,
                Year = new PersianDateTime(g.First().Year).Year
            }).ToList();

            return Json(new
            {
                years = totalEmployees.GroupBy(a => a.Year).Select(g => new
                {
                    Key = g.Key,
                    Year = g.First().Year,
                }).Select(a => a.Year),

                totalEmployees = totalEmployees.Select(a => a.Value),
                centralOfficeEmployees = centralOfficeEmployees.Select(a => a.Value),
                projectEmployees = projectEmployees.Select(a => a.Value)
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }




        [DontWrapResult]
        public async Task<ActionResult> GetCompanyWarrantyBalanceChartDatas(int yearId)
        {
            var userId = AbpSession.UserId.Value;

            var warrantyBalances = await _warrantyBalanceAppService.GetCompanyWarrantyBalances(yearId);

            var pishDaryaft = warrantyBalances.GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            {
                Key = g.Key,
                Amount = g.Sum(b => b.PreReceived),
                Year = g.First().Year
            }).ToList();


            var ejrayeTahodat = warrantyBalances.GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            {
                Key = g.Key,
                Amount = g.Sum(b => b.ObligationExecution),
                Year = g.First().Year
            }).ToList();


            var vajhoZeman = warrantyBalances.GroupBy(a => new PersianDateTime(a.Year).Year).Select(g => new
            {
                Key = g.Key,
                Amount = g.Sum(b => b.GuaranteeDeduction),
                Year = g.First().Year
            }).ToList();

            return Json(new
            {
                years = warrantyBalances.GroupBy(a => a.Year).Select(g => new
                {
                    Key = g.Key,
                    Year = new PersianDateTime(g.First().Year).Year
                }).Select(a => a.Year),
                pishDaryaft = pishDaryaft.Select(a => a.Amount / 1000000),
                ejrayeTahodat = ejrayeTahodat.Select(a => a.Amount / 1000000),
                vajhoZeman = vajhoZeman.Select(a => a.Amount / 1000000)
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [DontWrapResult]
        public async Task<ActionResult> GetCompanyLaborInventoryMonthlyChartDatas(int year)
        {
            var labors = await _laborInventoryAppService.GetYearCompanyLaborInventories(year);
            //var laborsInYear = labors.FirstOrDefault(a => new PersianDateTime(a.Year).Year == year);
            int CentralOfficeProjectId = 75;
            var list = new List<LaborInventoryMonthlyViewModel>();

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)1),
                Total = labors.Sum(a => a.Month1_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month1_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month1_Amount)
            });

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)2),
                Total = labors.Sum(a => a.Month2_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month2_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month2_Amount)
            });

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)3),
                Total = labors.Sum(a => a.Month3_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month3_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month3_Amount)
            });

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)4),
                Total = labors.Sum(a => a.Month4_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month4_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month4_Amount)
            });


            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)5),
                Total = labors.Sum(a => a.Month5_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month5_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month5_Amount)
            });


            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)6),
                Total = labors.Sum(a => a.Month6_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month6_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month6_Amount)
            });

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)7),
                Total = labors.Sum(a => a.Month7_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month7_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month7_Amount)
            });

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)8),
                Total = labors.Sum(a => a.Month8_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month8_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month8_Amount)
            });

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)9),
                Total = labors.Sum(a => a.Month9_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month9_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month9_Amount)
            });

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)10),
                Total = labors.Sum(a => a.Month10_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month10_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month10_Amount)
            });


            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)11),
                Total = labors.Sum(a => a.Month11_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month11_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month11_Amount)
            });

            list.Add(new LaborInventoryMonthlyViewModel()
            {
                Month = EnumExtensions.GetDisplayName((SelectiveMonth)12),
                Total = labors.Sum(a => a.Month12_Amount),
                CentralOffice = labors.Where(a => a.ProjectId == CentralOfficeProjectId).Sum(a => a.Month12_Amount),
                Project = labors.Where(a => a.ProjectId != CentralOfficeProjectId).Sum(a => a.Month12_Amount)
            });

            return Json(new
            {
                months = list.Select(a => a.Month),
                totalEmployees = list.Select(a => a.Total),
                centralOfficeEmployees = list.Select(a => a.CentralOffice),
                projectEmployees = list.Select(a => a.Project)
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> GetHrMonthlyByProjectChartData(int year, int month)
        {
            var userId = AbpSession.UserId.Value;
            var random = new Random();
            var laborInventories = (await _laborInventoryAppService.GetYearCompanyLaborInventories(year)).ToList();

            var currentMonthTotalYear = laborInventories.Sum(a => a.Month1_Amount);
            var labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
            {
                Key = g.First().Project.Title,
                Value = g.Sum(a => a.Month1_Amount),
                Color = String.Format("#{0:X6}", random.Next(0x1000000))
            }).OrderByDescending(a => a.Value).ToList();

            switch (month)
            {
                case 1:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month1_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month1_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;

                case 2:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month2_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month2_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;

                case 3:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month3_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month3_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;

                case 4:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month4_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month4_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;

                case 5:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month5_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month5_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;
                case 6:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month6_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month6_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;
                case 7:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month7_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month7_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;
                case 8:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month8_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month8_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;
                case 9:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month9_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month9_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;
                case 10:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month10_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month10_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;
                case 11:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month11_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month11_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;
                case 12:
                    currentMonthTotalYear = laborInventories.Sum(a => a.Month12_Amount);
                    labors = laborInventories.GroupBy(a => a.ProjectId).Select(g => new
                    {
                        Key = g.First().Project.Title,
                        Value = g.Sum(a => a.Month12_Amount) / currentMonthTotalYear * 100,
                        Color = String.Format("#{0:X6}", random.Next(0x1000000))
                    }).OrderByDescending(a => a.Value).ToList();
                    break;

                default:
                    break;
            }


            return Json(new
            {
                projects = labors.Select(a => a.Key),
                colors = labors.Select(a => a.Color),
                monthlySales = labors.Select(a => Math.Round(a.Value))
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ProjectsDocumentations()
        {
            var Projects = await _project_User_MappingAppService.GetUserProjects(AbpSession.UserId.Value);

            ViewBag.ProjectId = Projects.Select(a => new ProjectViewModel()
            {
                Id = a.ProjectId,
                Title = a.Project.Title
            }).ToList();

            return View();
        }

        [DontWrapResult]
        public async Task<ActionResult> AFCDocuments(int? ProjectID , string Year = "")
        {
            DateTime? ConvertedStartYear = null;
            DateTime? ConvertedEndYear = null;

            if (Year != null)
            {
                string StartDate = Year + "/01/01";
                string EndDate = Year + "/12/29";

                ConvertedStartYear = PersianDateTime.Parse(StartDate);
                ConvertedEndYear = PersianDateTime.Parse(EndDate);
            }

            var Info = await _projectsTasksAppService.AFCDocumentsFilter(ProjectID , ConvertedStartYear , ConvertedEndYear);

            string[] DiciplineTypes = { "Civil", "Electrical", "Instrument", "Mechanical", "Piping", "Process", "Quality" };

            int[] DiciplineValues = {
                Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)1).Count(),
                Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)2).Count(),
                Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)3).Count(),
                Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)4).Count(),
                Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)5).Count(),
                Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)6).Count(),
                Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)7).Count()
            };

            return Json(new { DiciplineTypes , DiciplineValues }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> AllDocuments(int? ProjectID, string Year = "", string Month = "")
        {
            string StartDate = "";
            string EndDate = "";

            DateTime? ConvertedStartYear = null;
            DateTime? ConvertedEndYear = null;

            var List = new List<int>();
            var Result = new List<ProjectsTasks>();

            if (Year != null && Month != null)
            {
                if (Month == "12" || Month == "11" || Month == "10") { StartDate = Year + "/" + Month + "/01"; } else { StartDate = Year + "/0" + Month + "/01"; };
                if (Month == "12") { EndDate = Year + "/" + Month + "/29"; } else if (Month == "11" || Month == "10") { EndDate = Year + "/" + Month + "/30"; } else if( Month == "9" || Month == "8" || Month == "7") { EndDate = Year + "/0" + Month + "/30"; } else { EndDate = Year + "/0" + Month + "/31"; }

                ConvertedStartYear = PersianDateTime.Parse(StartDate);
                ConvertedEndYear = PersianDateTime.Parse(EndDate);
            }
            else if (Year != null && Month == null)
            {
                StartDate = Year + "/01/01";
                EndDate = Year + "/12/29";

                ConvertedStartYear = PersianDateTime.Parse(StartDate);
                ConvertedEndYear = PersianDateTime.Parse(EndDate);
            }

            if (ProjectID.HasValue) 
            {
                var DocumentsSearch = await _projectsTasksAppService.GetAllProjectsTasks();
                var DocumentsFilter = DocumentsSearch.Where(a => a.ProjectID == ProjectID);
                var DocumentsResult = DocumentsFilter.Select(a => a.Id).ToList();

                foreach (var Item in DocumentsResult) { List.Add(Item); }
            }
            else
            {
               var DocumentSearch = await _projectsTasksAppService.GetAllProjectsTasks();
               var DocumentsResult = DocumentSearch.Select(a => a.Id).ToList();

               foreach (var Item in DocumentsResult) { List.Add(Item); }
            }

            var RevisionsFilter = await _projectsTasksRevisionsAppService.AllDocumentsFilter( List, ConvertedStartYear , ConvertedEndYear);

            var RevisionsResult = RevisionsFilter.Select(a => a.TaskID).ToList();

            foreach (var Item in RevisionsResult)
            {
                var DocumentResult = _projectsTasksAppService.GetSpecificProjectsTask(Item).Result.FirstOrDefault();

                Result.Add(new ProjectsTasks
                {
                    Id = DocumentResult.Id,
                    ProjectID = DocumentResult.ProjectID,
                    ProjectCode = DocumentResult.ProjectCode,
                    CompanyName = DocumentResult.CompanyName,
                    DocumentTitle = DocumentResult.DocumentTitle,
                    DocumentNumber = DocumentResult.DocumentNumber,
                    Dicipline = DocumentResult.Dicipline,
                    ResponsiblePerson = DocumentResult.ResponsiblePerson,
                    DocumentType = DocumentResult.DocumentType,
                    Description = DocumentResult.Description,
                    WeightFactor = DocumentResult.WeightFactor,
                    Progress = DocumentResult.Progress,
                    OriginalDuration = DocumentResult.OriginalDuration,
                    SourceOfItem = DocumentResult.SourceOfItem,
                    ManPower = DocumentResult.ManPower,
                    Critical = DocumentResult.Critical
                });
            }

            string[] DiciplineTypes = { "Civil", "Electrical", "Instrument", "Mechanical", "Piping", "Process", "Quality" };

            int[] DiciplineValues = {
                Result.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)1).Count(),
                Result.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)2).Count(),
                Result.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)3).Count(),
                Result.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)4).Count(),
                Result.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)5).Count(),
                Result.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)6).Count(),
                Result.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)7).Count()
            };

            return Json(new { DiciplineTypes, DiciplineValues , }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> AverageAFCDocuments(int? ProjectID, string Year = "")
        {

            List<int> DiciplineValues = new List<int>();

            DateTime? ConvertedStartYear = null;
            DateTime? ConvertedEndYear = null;


            ProjectsTasksViewModel.AverageAFCDocuments AverageAFCDocuments = delegate (List<ProjectsTasks> List , int DiciplineType , int? ProjectIDs , DateTime? StartDate , DateTime? EndDate)
            {
                var RevisionsTotal = 0;

                var DocumentsIDs = List.Select(a => a.Id);

                foreach(var ID in DocumentsIDs)
                {
                    var Revision = _projectsTasksRevisionsAppService.GetSpecificProjectsTaskRevisions(ID);

                    var RevisionNumber = Convert.ToInt32(Revision.Result.Select(a => a.RevisionNumber).LastOrDefault());

                    RevisionsTotal = RevisionsTotal + RevisionNumber;
                }

                var AllDocuments = _projectsTasksAppService.SelectedAFCDocument(ProjectIDs, StartDate, EndDate);

                var Result = (float)RevisionsTotal / (float)AllDocuments.Result.Where(a => a.Dicipline ==(ProjectsTasksDiciplineTypes)DiciplineType && a.LastStatus == (ProjectsTasksStatusTypes)1).Count();

                DiciplineValues.Add((int)Result);
            };

            if (Year != null )
            {
                var StartDate = Year + "/01/01";
                var EndDate = Year + "/12/29";

                ConvertedStartYear = PersianDateTime.Parse(StartDate);
                ConvertedEndYear = PersianDateTime.Parse(EndDate);
            }

            var Info = await _projectsTasksAppService.AverageAFCDocuments(ProjectID , ConvertedStartYear , ConvertedEndYear);

            string[] DiciplineTypes = { "Civil", "Electrical", "Instrument", "Mechanical", "Piping", "Process", "Quality" };

            if (Info.Count() > 0)
            {
                
                AverageAFCDocuments(Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)1 && a.LastStatus == (ProjectsTasksStatusTypes)1).ToList() , 1 , ProjectID, ConvertedStartYear , ConvertedEndYear); //Civil
                AverageAFCDocuments(Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)2 && a.LastStatus == (ProjectsTasksStatusTypes)1).ToList() , 2 , ProjectID, ConvertedStartYear , ConvertedEndYear); //Electrical
                AverageAFCDocuments(Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)3 && a.LastStatus == (ProjectsTasksStatusTypes)1).ToList() , 3 , ProjectID, ConvertedStartYear , ConvertedEndYear); //Instrument
                AverageAFCDocuments(Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)4 && a.LastStatus == (ProjectsTasksStatusTypes)1).ToList() , 4 , ProjectID, ConvertedStartYear , ConvertedEndYear); //Mechanical
                AverageAFCDocuments(Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)5 && a.LastStatus == (ProjectsTasksStatusTypes)1).ToList() , 5 , ProjectID, ConvertedStartYear , ConvertedEndYear); //Piping
                AverageAFCDocuments(Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)6 && a.LastStatus == (ProjectsTasksStatusTypes)1).ToList() , 6 , ProjectID, ConvertedStartYear , ConvertedEndYear); //Process
                AverageAFCDocuments(Info.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)7 && a.LastStatus == (ProjectsTasksStatusTypes)1).ToList() , 7 , ProjectID, ConvertedStartYear , ConvertedEndYear); //Quality


                return Json(new { DiciplineTypes, DiciplineValues }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }

            else 
            {
                return Json(new { DiciplineTypes, DiciplineValues }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
            }
        }

        [DontWrapResult]
        public async Task<ActionResult> AverageToGetAFC(int? ProjectID, string Year = "")
        {
            DateTime? ConvertedStartYear = null;
            DateTime? ConvertedEndYear = null;

            TimeSpan CivilResult = TimeSpan.FromSeconds(0);
            TimeSpan ElectricalResult = TimeSpan.FromSeconds(0);
            TimeSpan InstrumentResult = TimeSpan.FromSeconds(0);
            TimeSpan MechanicalResult = TimeSpan.FromSeconds(0);
            TimeSpan PipingResult = TimeSpan.FromSeconds(0);
            TimeSpan ProcessResult = TimeSpan.FromSeconds(0);
            TimeSpan QualityControlResult = TimeSpan.FromSeconds(0);

            string[] DiciplineTypes = { "Civil", "Electrical", "Instrument", "Mechanical", "Piping", "Process", "Quality" };

            if (Year != null)
            {
                var StartDate = Year + "/01/01";
                var EndDate = Year + "/12/29";

                ConvertedStartYear = PersianDateTime.Parse(StartDate);
                ConvertedEndYear = PersianDateTime.Parse(EndDate);
            }

            var DocumentsFilter = await _projectsTasksAppService.AverageToGetAFC(ProjectID, ConvertedStartYear, ConvertedEndYear);

            var DocumentsResult = DocumentsFilter.Select(a => a.Id);

            foreach(var Item in DocumentsResult)
            {
                var RevisionsFilter = await _projectsTasksRevisionsAppService.GetSpecificProjectsTaskRevisions(Item);

                var TaskID = RevisionsFilter.Select(a => a.TaskID).FirstOrDefault();
                var FirstTRNo = RevisionsFilter.Select(a => a.TransmitalDate).FirstOrDefault();
                var LastTRNo = RevisionsFilter.Select(a => a.TransmitalDate).LastOrDefault();

                var Document = await _projectsTasksAppService.GetSpecificProjectsTask(TaskID);

                if (Document.Select(a => a.Dicipline).FirstOrDefault() == (ProjectsTasksDiciplineTypes)1) { TimeSpan Span = (TimeSpan)(LastTRNo - FirstTRNo); CivilResult = new TimeSpan(CivilResult.Ticks + Span.Ticks); };
                if (Document.Select(a => a.Dicipline).FirstOrDefault() == (ProjectsTasksDiciplineTypes)2) { TimeSpan Span = (TimeSpan)(LastTRNo - FirstTRNo); ElectricalResult = new TimeSpan(ElectricalResult.Ticks + Span.Ticks); };
                if (Document.Select(a => a.Dicipline).FirstOrDefault() == (ProjectsTasksDiciplineTypes)3) { TimeSpan Span = (TimeSpan)(LastTRNo - FirstTRNo); InstrumentResult = new TimeSpan(InstrumentResult.Ticks + Span.Ticks); };
                if (Document.Select(a => a.Dicipline).FirstOrDefault() == (ProjectsTasksDiciplineTypes)4) { TimeSpan Span = (TimeSpan)(LastTRNo - FirstTRNo); MechanicalResult = new TimeSpan(MechanicalResult.Ticks + Span.Ticks); };
                if (Document.Select(a => a.Dicipline).FirstOrDefault() == (ProjectsTasksDiciplineTypes)5) { TimeSpan Span = (TimeSpan)(LastTRNo - FirstTRNo); PipingResult = new TimeSpan(PipingResult.Ticks + Span.Ticks); };
                if (Document.Select(a => a.Dicipline).FirstOrDefault() == (ProjectsTasksDiciplineTypes)6) { TimeSpan Span = (TimeSpan)(LastTRNo - FirstTRNo); ProcessResult = new TimeSpan(ProcessResult.Ticks + Span.Ticks); };
                if (Document.Select(a => a.Dicipline).FirstOrDefault() == (ProjectsTasksDiciplineTypes)7) { TimeSpan Span = (TimeSpan)(LastTRNo - FirstTRNo); QualityControlResult = new TimeSpan(QualityControlResult.Ticks + Span.Ticks); };
            };

            //Get AFCRevisions Then Get Documents That Have AFCRevisions Then Specify Them By Dicipline Types

            var CountAFCCivil = DocumentsFilter.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)1).Count();
            var CountAFCElectrical = DocumentsFilter.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)2).Count();
            var CountAFCInstrument = DocumentsFilter.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)3).Count();
            var CountAFCMechanical = DocumentsFilter.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)4).Count();
            var CountAFCPiping = DocumentsFilter.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)5).Count();
            var CountAFCProcess = DocumentsFilter.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)6).Count();
            var CountAFCQualityControl = DocumentsFilter.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)7).Count();

            float[] DiciplineValues = { (float)CivilResult.Days / (float)CountAFCCivil, (float)ElectricalResult.Days / (float)CountAFCElectrical, (float)InstrumentResult.Days / (float)CountAFCInstrument, (float)MechanicalResult.Days / (float)CountAFCMechanical, (float)PipingResult.Days / (float)CountAFCPiping, (float)ProcessResult.Days / (float)CountAFCProcess , (float)QualityControlResult.Days / (float)CountAFCQualityControl };

            return Json(new { DiciplineTypes, DiciplineValues }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> DiciplineProduceProcess(int? ProjectID = null, int? Dicipline = null, string StartYear = null , string StartMonth = null)
        {

            string StartDate = "";

            DateTime ConvertedStartDate = default(DateTime);

            List<string> ChartTitles = new List<string>();

            List<int> ChartValues = new List<int>();

            var CurrentDate = new PersianDateTime(DateTime.Now);



            ProjectsTasksViewModel.PersianDigitToEnglish PersianDigitToEnglish = delegate (string Parameter)
            {
                Dictionary<char, char> LettersDictionary = new Dictionary<char, char> { ['۰'] = '0', ['۱'] = '1', ['۲'] = '2', ['۳'] = '3', ['۴'] = '4', ['۵'] = '5', ['۶'] = '6', ['۷'] = '7', ['۸'] = '8', ['۹'] = '9', ['/'] = '/' };

                foreach (var item in Parameter)
                {
                    Parameter = Parameter.Replace(item, LettersDictionary[item]);
                }
                return Parameter.ToString();
            };

            ProjectsTasksViewModel.DiciplineProduceProcess DiciplineProduceProcessFunction = delegate (List<ProjectsTasksRevisions> FilterRevisionsList, int? ImportYear , int? ImportStartMonth , int? ImportEndMonth )
             {

                 for (int MonthCounter = (int)ImportStartMonth ; MonthCounter <= ImportEndMonth ; MonthCounter++)
                 {
                     var ForLoopStartDate = "";

                     var ForLoopEndDate = "";

                     if (MonthCounter == 12) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/29"; }
                     else if (MonthCounter == 11) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/30"; }
                     else if (MonthCounter == 10) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/30"; }
                     else if (MonthCounter == 9) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/30"; }
                     else if (MonthCounter == 8) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/30"; }
                     else if (MonthCounter == 7) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/30"; }
                     else if (MonthCounter == 6) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/31"; }
                     else if (MonthCounter == 5) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/31"; }
                     else if (MonthCounter == 4) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/31"; }
                     else if (MonthCounter == 3) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/31"; }
                     else if (MonthCounter == 2) { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/31"; }
                     else { ForLoopStartDate = ImportYear + "/" + MonthCounter + "/01"; ForLoopEndDate = ImportYear + "/" + MonthCounter + "/31"; }

                     var Result = FilterRevisionsList.Where(a => a.TransmitalDate >= PersianDateTime.Parse(ForLoopStartDate) && a.TransmitalDate <= PersianDateTime.Parse(ForLoopEndDate)).Count();

                     switch (MonthCounter)
                     {
                         case 1:
                             ChartTitles.Add("Farvardin " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 2:
                             ChartTitles.Add("Ordibehesht " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 3:
                             ChartTitles.Add("Khordad " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 4:
                             ChartTitles.Add("Tir " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 5:
                             ChartTitles.Add("Mordad " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 6:
                             ChartTitles.Add("Shahrivar " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 7:
                             ChartTitles.Add("Mehr " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 8:
                             ChartTitles.Add("Aban " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 9:
                             ChartTitles.Add("Azar " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 10:
                             ChartTitles.Add("Dey " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 11:
                             ChartTitles.Add("Bahman " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         case 12:
                             ChartTitles.Add("Esfand " + ImportYear);
                             ChartValues.Add(Result);
                             break;
                         default:
                             break;
                     }
                 }
             };



            if (StartYear == null && StartMonth == null)
            {
                StartYear = PersianDigitToEnglish(new PersianDateTime(DateTime.Now.AddYears(-1)).ToString().Split("/")[0].ToString()); //It Returns One Year Before Now 

                StartMonth = "01";

                StartDate = StartYear + "/" + StartMonth + "/01";

                ConvertedStartDate = PersianDateTime.Parse(StartDate);
            }
            else if (StartYear != null && StartMonth != null)
            {
                if (StartMonth == "12" || StartMonth == "11" || StartMonth == "10") { StartDate = StartYear + "/" + StartMonth + "/01"; } else { StartDate = StartYear + "/0" + StartMonth + "/01"; };

                ConvertedStartDate = PersianDateTime.Parse(StartDate);
            }
            else if (StartYear != null && StartMonth == null)
            {
                StartDate = StartYear + "/01/01";

                ConvertedStartDate = PersianDateTime.Parse(StartDate);
            }



            var FilterDocuments = await _projectsTasksAppService.DiciplineProduceProcess(ProjectID, (ProjectsTasksDiciplineTypes?)Dicipline);

            var DocumentIDs = FilterDocuments.Select(a => a.Id).ToList();

            var FilterRevisions = await _projectsTasksRevisionsAppService.DiciplineProduceProcess(DocumentIDs, ConvertedStartDate);

            for (int YearCounter = Convert.ToInt32(new PersianDateTime(ConvertedStartDate).Year.ToString()) ; YearCounter <= Convert.ToInt32(new PersianDateTime(DateTime.Now).Year.ToString()) ; YearCounter++)
            {
                int ImportYear = 0;
                int ImportStartMonth = 0;
                int ImportEndMonth = 0;

                //If The Chosen Year And Year That We Are In Are The Same - OR - Checking The Last Year
                if (Convert.ToInt32(new PersianDateTime(ConvertedStartDate).Year.ToString()) == Convert.ToInt32(new PersianDateTime(DateTime.Now).Year.ToString()) || YearCounter == Convert.ToInt32(new PersianDateTime(DateTime.Now).Year.ToString()))
                {
                    ImportYear = YearCounter;
                    ImportStartMonth = 1;
                    ImportEndMonth = Convert.ToInt32(new PersianDateTime(DateTime.Now).Month.ToString());

                    DiciplineProduceProcessFunction(FilterRevisions, ImportYear, ImportStartMonth, ImportEndMonth);
                }
                //Checking The First Year 
                else if (YearCounter == Convert.ToInt32(new PersianDateTime(ConvertedStartDate).Year.ToString()))
                {
                    if (StartMonth == null)  { ImportYear = YearCounter; ImportStartMonth = 1; ImportEndMonth = 12; }
                    
                    else if (StartMonth.Length > 0) { ImportYear = YearCounter; ImportStartMonth = Convert.ToInt32(new PersianDateTime(ConvertedStartDate).Month.ToString()); ImportEndMonth = 12; }

                    DiciplineProduceProcessFunction(FilterRevisions, ImportYear , ImportStartMonth , ImportEndMonth);
                }
                //The Years Between First And Last Year
                else
                {
                    ImportYear = YearCounter;
                    ImportStartMonth = 1;
                    ImportEndMonth = 12;

                    DiciplineProduceProcessFunction(FilterRevisions, ImportYear, ImportStartMonth , ImportEndMonth);
                }
            }
            return Json(new { ChartTitles , ChartValues }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> AverageProjectsDocuments(string StartDate = null, string EndDate = null) 
        {
            //var InputYear = PersianDateTime.Parse(Year);

            DateTime? ConvertedStartDate = default(DateTime?);
            DateTime? ConvertedEndDate = default(DateTime?);

            if (StartDate != null) { ConvertedStartDate = PersianDateTime.Parse(StartDate); }
            
            if (EndDate != null) { ConvertedEndDate = PersianDateTime.Parse(EndDate); }


            var ProjectsNames = new List<string>();

            var ProjectsAverageDocuments = new List<string>();


            var ProjectsIDs = await _projectAppService.GetAllProjects();

            var AllRevisions = await _projectsTasksRevisionsAppService.AllRevisionsInPeriodOfTime(ConvertedStartDate , ConvertedEndDate);

            foreach (var Item in ProjectsIDs)
            {
                var ProjectsDocumentsResult = await _projectsTasksAppService.GetDocumentsByProjectID(Item.Id);

                var ProjectDocumentsIDs = ProjectsDocumentsResult.Select(a => a.Id).ToList();


                var ProjectDocumentsRevisions = await _projectsTasksRevisionsAppService.AverageProjectsDocumentsProducedRevisions(ProjectDocumentsIDs, ConvertedStartDate, ConvertedEndDate);

                if (ProjectDocumentsRevisions.Count() > 0)
                {
                    ProjectsNames.Add(Item.Title);

                    float Calculation = (float)ProjectDocumentsRevisions.Count() / (float)AllRevisions.Count() * 100;

                    ProjectsAverageDocuments.Add( Calculation.ToString("0.00") );
                }
            }

         return Json(new { ProjectsNames , ProjectsAverageDocuments }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> DocumentsStatusTable()
        {
            //var InputYear = PersianDateTime.Parse(Year);

            var ProjectsNames = new List<string>();

            var ProjectsAverageDocuments = new List<float>();


            var ProjectsIDs = await _projectAppService.GetAllProjects();

            var AllDocuments = await _projectsTasksAppService.GetAllProjectsTasks();

            foreach (var Item in ProjectsIDs)
            {
                var Info = await _projectsTasksAppService.GetDocumentsByProjectID(Item.Id);

                if (Info.Count() > 0)
                {
                    ProjectsNames.Add(Item.Title);

                    ProjectsAverageDocuments.Add(Info.Count() * 100 / AllDocuments.Count());
                }
            }

            return Json(new { ProjectsNames, ProjectsAverageDocuments }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [DontWrapResult]
        public async Task<ActionResult> DocumentsStatus(int? ProjectID)
        {

            DashboardDocsStatusViewModel.ActionTypeCounter ActionCounter = delegate (List<ProjectsTasks> Input)
            {
                var List = new List<DashboardDocsStatusViewModel>();

                List.Add(new DashboardDocsStatusViewModel
                {
                    IssuedByPM = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)5).Count(),

                    CommentedByFSTCO = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)3).Count(),

                    CommentedByIDOM = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)4).Count(),

                    ApprovedByFSTCO = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)1).Count(),

                    ApprovedByIDOM = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)2).Count(),

                    AppWithNotesByFSTCO = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)16).Count(),

                    AppWithNotesByIDOM = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)17).Count(),

                    NotIssued = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)6).Count(),

                    Delete = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)7).Count(),

                    Total = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)1 || a.LastAction == (ProjectsTasksActionTypes)2 || a.LastAction == (ProjectsTasksActionTypes)3 || a.LastAction == (ProjectsTasksActionTypes)4 || a.LastAction == (ProjectsTasksActionTypes)5 || a.LastAction == (ProjectsTasksActionTypes)6 || a.LastAction == (ProjectsTasksActionTypes)7 || a.LastAction == (ProjectsTasksActionTypes)16 || a.LastAction == (ProjectsTasksActionTypes)17).Count(),

                    TotalIssued = Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)1 || a.LastAction == (ProjectsTasksActionTypes)2 || a.LastAction == (ProjectsTasksActionTypes)3 || a.LastAction == (ProjectsTasksActionTypes)4 || a.LastAction == (ProjectsTasksActionTypes)5 || a.LastAction == (ProjectsTasksActionTypes)6 || a.LastAction == (ProjectsTasksActionTypes)7 || a.LastAction == (ProjectsTasksActionTypes)16 || a.LastAction == (ProjectsTasksActionTypes)17).Count() - Input.Where(a => a.LastAction == (ProjectsTasksActionTypes)6 || a.LastAction == (ProjectsTasksActionTypes)7).Count()
                });

                return List;
            };

            DashboardDocsStatusViewModel.DocumentTypeCounter DocumentTypeCounter = delegate (string Title , List<ProjectsTasks> Input)
            {
                var List = new List<DashboardDocsStatusViewModel>();


                var BasicDesign = ActionCounter(Input.Where(a => a.DocumentType == (ProjectsTasksDocumentTypes)11).ToList());

                List.Add(new DashboardDocsStatusViewModel { DiciplineType = Title , DocumentType = "Basic Design", IssuedByPM = BasicDesign.Select(a => a.IssuedByPM).FirstOrDefault() , CommentedByFSTCO = BasicDesign.Select(a => a.CommentedByFSTCO).FirstOrDefault(), CommentedByIDOM = BasicDesign.Select(a => a.CommentedByIDOM).FirstOrDefault(), ApprovedByFSTCO = BasicDesign.Select(a => a.ApprovedByFSTCO).FirstOrDefault(), ApprovedByIDOM = BasicDesign.Select(a => a.ApprovedByIDOM).FirstOrDefault(), AppWithNotesByFSTCO = BasicDesign.Select(a => a.AppWithNotesByFSTCO).FirstOrDefault(), AppWithNotesByIDOM = BasicDesign.Select(a => a.AppWithNotesByIDOM).FirstOrDefault(), NotIssued = BasicDesign.Select(a => a.NotIssued).FirstOrDefault(), Delete = BasicDesign.Select(a => a.Delete).FirstOrDefault() , Total = BasicDesign.Select(a => a.Total).FirstOrDefault() , TotalIssued = BasicDesign.Select(a => a.TotalIssued).FirstOrDefault() });

                var DetailDesign = ActionCounter(Input.Where(a => a.DocumentType == (ProjectsTasksDocumentTypes)12).ToList());

                List.Add(new DashboardDocsStatusViewModel { DiciplineType = Title, DocumentType = "Detail Design", IssuedByPM = DetailDesign.Select(a => a.IssuedByPM).FirstOrDefault(), CommentedByFSTCO = DetailDesign.Select(a => a.CommentedByFSTCO).FirstOrDefault(), CommentedByIDOM = DetailDesign.Select(a => a.CommentedByIDOM).FirstOrDefault(), ApprovedByFSTCO = DetailDesign.Select(a => a.ApprovedByFSTCO).FirstOrDefault(), ApprovedByIDOM = DetailDesign.Select(a => a.ApprovedByIDOM).FirstOrDefault(), AppWithNotesByFSTCO = DetailDesign.Select(a => a.AppWithNotesByFSTCO).FirstOrDefault(), AppWithNotesByIDOM = DetailDesign.Select(a => a.AppWithNotesByIDOM).FirstOrDefault() , NotIssued = DetailDesign.Select(a => a.NotIssued).FirstOrDefault(), Delete = DetailDesign.Select(a => a.Delete).FirstOrDefault() , Total = DetailDesign.Select(a => a.Total).FirstOrDefault() , TotalIssued = DetailDesign.Select(a => a.TotalIssued).FirstOrDefault() });

                var Procurement = ActionCounter(Input.Where(a => a.DocumentType == (ProjectsTasksDocumentTypes)13).ToList());

                List.Add(new DashboardDocsStatusViewModel { DiciplineType = Title, DocumentType = "Procurement Engineering", IssuedByPM = Procurement.Select(a => a.IssuedByPM).FirstOrDefault(), CommentedByFSTCO = Procurement.Select(a => a.CommentedByFSTCO).FirstOrDefault(), CommentedByIDOM = Procurement.Select(a => a.CommentedByIDOM).FirstOrDefault(), ApprovedByFSTCO = Procurement.Select(a => a.ApprovedByFSTCO).FirstOrDefault(), ApprovedByIDOM = Procurement.Select(a => a.ApprovedByIDOM).FirstOrDefault(), AppWithNotesByFSTCO = Procurement.Select(a => a.AppWithNotesByFSTCO).FirstOrDefault(), AppWithNotesByIDOM = Procurement.Select(a => a.AppWithNotesByIDOM).FirstOrDefault() , NotIssued = Procurement.Select(a => a.NotIssued).FirstOrDefault(), Delete = Procurement.Select(a => a.Delete).FirstOrDefault() , Total = Procurement.Select(a => a.Total).FirstOrDefault() , TotalIssued = Procurement.Select(a => a.TotalIssued).FirstOrDefault() });

                return List;
            };

            List<ProjectsTasks> AllDocuments = new List<ProjectsTasks>();

            if (!ProjectID.HasValue)
            {
                AllDocuments = await _projectsTasksAppService.GetAllProjectsTasks();
            }
            else
            {
                AllDocuments = await _projectsTasksAppService.GetDocumentsByProjectID((int)ProjectID);
            }

            var Process = AllDocuments.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)6).ToList();

            var Mechanical = AllDocuments.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)4).ToList();

            var CivilAndArchitecture = AllDocuments.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)1).ToList();

            var Electrical = AllDocuments.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)2).ToList();

            var InstrumentAndControl = AllDocuments.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)3).ToList();

            var Piping = AllDocuments.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)5).ToList();

            var QualityControl = AllDocuments.Where(a => a.Dicipline == (ProjectsTasksDiciplineTypes)7).ToList();

            var AllDicipline = AllDocuments.ToList();

            //All Dicipline


            List<DashboardDocsStatusViewModel>[] Result = { 
                DocumentTypeCounter("Process" , Process) ,
                DocumentTypeCounter("Mechanical" , Mechanical),
                DocumentTypeCounter("Civil & Arichitecture" , CivilAndArchitecture),
                DocumentTypeCounter("Electrical" , Electrical),
                DocumentTypeCounter("Instrument & Control" , InstrumentAndControl),
                DocumentTypeCounter("Piping" , Piping),
                DocumentTypeCounter("Quality Control" , QualityControl),
                DocumentTypeCounter("All Dicipline" , AllDicipline)
            };

            return Json(Result);
        }

        [DontWrapResult]
        public async Task<ActionResult> TotalDocumentsStatus(int? ProjectID)
        {
            List<ProjectsTasks> AllDocuments = new List<ProjectsTasks>();

            if (!ProjectID.HasValue)
            {
                AllDocuments = await _projectsTasksAppService.GetAllProjectsTasks();
            }
            else
            {
                AllDocuments = await _projectsTasksAppService.GetDocumentsByProjectID((int)ProjectID);
            }

            int[] Result =
            {
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)5).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)3).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)4).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)1).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)2).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)6).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)7).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)16).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)17).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)1 || a.LastAction == (ProjectsTasksActionTypes)2 || a.LastAction == (ProjectsTasksActionTypes)3 || a.LastAction == (ProjectsTasksActionTypes)4 || a.LastAction == (ProjectsTasksActionTypes)5 || a.LastAction == (ProjectsTasksActionTypes)6 || a.LastAction == (ProjectsTasksActionTypes)7 || a.LastAction == (ProjectsTasksActionTypes)16 || a.LastAction == (ProjectsTasksActionTypes)17).Count(),
                AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)1 || a.LastAction == (ProjectsTasksActionTypes)2 || a.LastAction == (ProjectsTasksActionTypes)3 || a.LastAction == (ProjectsTasksActionTypes)4 || a.LastAction == (ProjectsTasksActionTypes)5 || a.LastAction == (ProjectsTasksActionTypes)6 || a.LastAction == (ProjectsTasksActionTypes)7 || a.LastAction == (ProjectsTasksActionTypes)16 || a.LastAction == (ProjectsTasksActionTypes)17).Count() - AllDocuments.Where(a => a.LastAction == (ProjectsTasksActionTypes)6 || a.LastAction == (ProjectsTasksActionTypes)7).Count()
            };

            return Json(Result);
        }
    }
}
