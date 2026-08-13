using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Web.Models;
using Nexora.PMPortal.Controllers;
using Nexora.PMPortal.Financial;
using Nexora.PMPortal.Financial.Dto;
using Nexora.PMPortal.Projects;
using Nexora.PMPortal.Web.ViewModels;
using Nexora.PMPortal.Web.ViewModels.ChartForm;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nexora.PMPortal.Web.Controllers
{
    [AbpMvcAuthorize]
    public class GrossProfitToNetController : PMPortalControllerBase
    {

        private readonly IProjectAppService _projectAppService;
        private readonly IGrossProfitToNetAppService _GrossProfitToNetAppService;

        public GrossProfitToNetController(IProjectAppService projectAppService, IGrossProfitToNetAppService GrossProfitToNetAppService)
        {
            _projectAppService = projectAppService;
            _GrossProfitToNetAppService = GrossProfitToNetAppService;
        }


        public IActionResult Index(int? lastPage = 1)
        {
            ViewBag.LastPageValue = lastPage;
            return View();
        }



        [DontWrapResult]
        public async Task<ActionResult> GrossProfitToNetRead([DataSourceRequest] DataSourceRequest request)
        {
            var userId = AbpSession.UserId;
            var projects = await _GrossProfitToNetAppService.GetAllGrossProfitToNets();

            var result = projects.Select(i => new GrossProfitToNetViewModel
            {
                Id = i.Id,
                Year = new PersianDateTime(i.Year).Year,
                NetProfit = i.NetProfit.ToString("N0"),
                GrossProfit = i.GrossProfit.ToString("N0"),
                SaleCost = i.SaleCost.ToString("N0"),
                OperationalProfit = i.OperationalProfit.ToString("N0"),
                FinancialCost = i.FinancialCost.ToString("N0"),
                NonOperationalCost = i.NonOperationalCost.ToString("N0"),
                OperationalCost = i.OperationalCost.ToString("N0"),
                IncomeTax = i.IncomeTax.ToString("N0")
            }).ToList();

            var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> Create()
        {
            var items = await _projectAppService.GetAllProjects();
            ViewBag.ProjectId = items.Select(a => new ProjectViewModel()
            {
                Id = a.Id,
                Title = a.Title
            }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GrossProfitToNetViewModel model)
        {
            var item = new GrossProfitToNetDto()
            {
                Year = new PersianDateTime(model.Year, 1, 1),
                NetProfit = decimal.Parse(model.NetProfit),
                GrossProfit = decimal.Parse(model.GrossProfit),
                SaleCost = decimal.Parse(model.SaleCost),
                OperationalProfit = decimal.Parse(model.OperationalProfit),
                FinancialCost = decimal.Parse(model.FinancialCost),
                NonOperationalCost = decimal.Parse(model.NonOperationalCost),
                OperationalCost = decimal.Parse(model.OperationalCost),
                IncomeTax = decimal.Parse(model.IncomeTax)
            };
            await _GrossProfitToNetAppService.Create(item);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id, int lastPage)
        {
            var items = await _projectAppService.GetAllProjects();
            ViewBag.ProjectId = items.Select(a => new ProjectViewModel()
            {
                Id = a.Id,
                Title = a.Title
            }).ToList();

            var item = await _GrossProfitToNetAppService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));

            var model = new GrossProfitToNetViewModel
            {
                Id = item.Id,
                Year = new PersianDateTime(item.Year).Year,
                NetProfit = item.NetProfit.ToString("0."),
                GrossProfit = item.GrossProfit.ToString("0."),
                SaleCost = item.SaleCost.ToString("0."),
                OperationalProfit = item.OperationalProfit.ToString("0."),
                FinancialCost = item.FinancialCost.ToString("0."),
                NonOperationalCost = item.NonOperationalCost.ToString("0."),
                OperationalCost = item.OperationalCost.ToString("0."),
                IncomeTax = item.IncomeTax.ToString("0.")
            };

            model.LastGridPage = lastPage;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GrossProfitToNetViewModel model)
        {
            var item = new GrossProfitToNetDto()
            {
                Id = model.Id,
                Year = new PersianDateTime(model.Year, 1, 1),
                NetProfit = decimal.Parse(model.NetProfit),
                GrossProfit = decimal.Parse(model.GrossProfit),
                SaleCost = decimal.Parse(model.SaleCost),
                OperationalProfit = decimal.Parse(model.OperationalProfit),
                FinancialCost = decimal.Parse(model.FinancialCost),
                NonOperationalCost = decimal.Parse(model.NonOperationalCost),
                OperationalCost = decimal.Parse(model.OperationalCost),
                IncomeTax = decimal.Parse(model.IncomeTax)
            };
            await _GrossProfitToNetAppService.Update(item);
            return RedirectToAction("Index", new { lastPage = model.LastGridPage });
        }


        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> GrossProfitToNetDestroy([DataSourceRequest] DataSourceRequest request, GrossProfitToNetViewModel item)
        {
            if (item != null)
            {
                await _GrossProfitToNetAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }
    }
}