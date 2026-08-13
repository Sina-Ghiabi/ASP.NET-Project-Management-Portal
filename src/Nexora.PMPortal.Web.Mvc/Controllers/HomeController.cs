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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nexora.PMPortal.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : PMPortalControllerBase
    {
        private readonly IPaymentRequestAppService _paymentRequestAppService;
        private readonly IReceiveBillAppService _receiveBillAppService;
        private readonly ITransactionAppService _transactionAppService;
        private readonly IProjectAppService _projectAppService;
        private readonly IContractorAppService _contractorAppService;
        private readonly IProject_User_MappingAppService _project_User_MappingAppService;

        public HomeController(IPaymentRequestAppService paymentRequestAppService,
            IReceiveBillAppService receiveBillAppService,
            ITransactionAppService transactionAppService,
            IProjectAppService projectAppService,
            IContractorAppService contractorAppService,
            IProject_User_MappingAppService project_User_MappingAppService
            )
        {
            _paymentRequestAppService = paymentRequestAppService;
            _receiveBillAppService = receiveBillAppService;
            _transactionAppService = transactionAppService;
            _projectAppService = projectAppService;
            _contractorAppService = contractorAppService;
            _project_User_MappingAppService = project_User_MappingAppService;
        }

        public async Task<ActionResult> Index(int? id)
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


        public async Task<ActionResult> Allocation()
        {
            var projects = await _projectAppService.GetAllProjects();

            var model = new IndexViewModel();

            model.ReportYear = (ReportYear)new PersianDateTime(DateTime.Now).Year;

            var userId = AbpSession.UserId.Value;
            var items = await _project_User_MappingAppService.GetUserProjects(userId);
            ViewBag.ProjectId = items.Select(a => new ProjectViewModel()
            {
                Id = a.ProjectId,
                Title = a.Project.Title
            }).ToList();

            model.MyProjectCount = items.Count;

            return View(model);
        }


        [DontWrapResult]
        public async Task<ActionResult> GetProjectChartDatas(int year, int month, int currencyId = (int)CurrencyType.IRR)
        {
            var userId = AbpSession.UserId.Value;

            var projects = await _project_User_MappingAppService.GetUserProjects(userId);

            var receives = await _receiveBillAppService.GetProjectsReceiveBills(projects.Select(a => a.ProjectId).ToList(), currencyId, year, month);

            var payments = await _transactionAppService.GetProjectsPayments(projects.Select(a => a.ProjectId).ToList(), currencyId, year, month);


            var receiveAmounts = projects.Select(a => new ProjectChartAmountViewModel
            {
                ProjectId = a.ProjectId,
                Amount = receives.Where(x => x.ProjectId == a.ProjectId).Sum(b => b.Amount)
            }).OrderBy(b => b.ProjectId).ToList();


            var paymentAmounts = projects.Select(a => new ProjectChartAmountViewModel
            {
                ProjectId = a.ProjectId,
                Amount = payments.Where(x => x.ProjectId == a.ProjectId).Sum(b => b.Amount)
            }).OrderBy(b => b.ProjectId).ToList();

            return Json(new
            {
                projects = projects.OrderBy(a => a.ProjectId).Select(a => a.Project.Title).ToList(),
                receiveAmounts = receiveAmounts.Select(a => Math.Ceiling(a.Amount / 1000000)).ToList(),
                paymentAmounts = paymentAmounts.Select(a => Math.Ceiling(a.Amount / 1000000)).ToList()
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [DontWrapResult]
        public async Task<ActionResult> GetProjectBoxDatas(int year, int[] projectId, int currencyId = (int)CurrencyType.IRR)
        {
            var userId = AbpSession.UserId.Value;

            var receives = await _receiveBillAppService.GetProjectsReceiveBills(projectId.ToList(), currencyId, year, 0);

            var payments = await _transactionAppService.GetProjectsPayments(projectId.ToList(), currencyId, year, 0);


            var projectReceiveAmounts = receives.Sum(b => b.Amount / 1000000);

            var projectPaymentAmounts = payments.Sum(b => b.Amount / 1000000);

            var balanceAmounts = projectReceiveAmounts - projectPaymentAmounts;

            return Json(new
            {
                projectReceiveAmounts = projectReceiveAmounts.ToString("N0"),
                projectPaymentAmounts = projectPaymentAmounts.ToString("N0"),
                balanceAmounts = balanceAmounts.ToString("N0")
            }, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }
    }
}
