using System;
using System.Collections.Generic;
using System.IO;
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
using Nexora.PMPortal.Web.ViewModels.ExcelModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml;

namespace Nexora.PMPortal.Web.Controllers
{
    [AbpMvcAuthorize]
    public class CostBenefitController : PMPortalControllerBase
    {

        private readonly IProjectAppService _projectAppService;
        private readonly ICostBenefitAppService _costBenefitAppService;

        public CostBenefitController(IProjectAppService projectAppService, ICostBenefitAppService costBenefitAppService)
        {
            _projectAppService = projectAppService;
            _costBenefitAppService = costBenefitAppService;
        }


        public IActionResult Index(int? lastPage = 1)
        {
            ViewBag.LastPageValue = lastPage;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(IFormFile fileExcel)
        {
            if (Request != null)
            {
                if ((fileExcel != null) && (fileExcel.Length != 0) && !string.IsNullOrEmpty(fileExcel.FileName))
                {
                    var fileName = Path.GetFileName(fileExcel.FileName);
                    var newFileName = "pm_" + Guid.NewGuid().ToString("N") + "_ExcelTemp" + "." + fileName.Split('.').LastOrDefault();
                    try
                    {
                        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", newFileName);
                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            await fileExcel.CopyToAsync(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }

                    var itemsList = new List<CostBenefitExcelRow>();

                    using (var package = new ExcelPackage(new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", newFileName))))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();

                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        var i = 1;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            try
                            {
                                if (workSheet.Cells[rowIterator, 1].Value == null || workSheet.Cells[rowIterator, 2].Value == null)
                                {
                                    continue;
                                }
                                var row = new CostBenefitExcelRow();
                                row.ProjectId = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                                row.Year = new PersianDateTime(Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value), 1, 1);
                                row.CostBenefitPeriod = workSheet.Cells[rowIterator, 3].Value != null ? Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value.ToString()) : 0;
                                row.ServiceType = workSheet.Cells[rowIterator, 4].Value != null ? Convert.ToInt32(workSheet.Cells[rowIterator, 4].Value.ToString()) : 0;
                                row.ApprovedSales = workSheet.Cells[rowIterator, 5].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()) : 0;
                                row.RecycleSales = workSheet.Cells[rowIterator, 6].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 6].Value.ToString()) : 0;
                                row.AllCost = workSheet.Cells[rowIterator, 7].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 7].Value.ToString()) : 0;
                                row.InflationRate = workSheet.Cells[rowIterator, 8].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 8].Value.ToString()) : 0;
                                row.GrossProfit = workSheet.Cells[rowIterator, 9].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 9].Value.ToString()) : 0;
                                itemsList.Add(row);
                                i++;
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Error = i + " - "
                                    + ex.Message;
                                return View();
                            }

                        }

                        foreach (var item in itemsList)
                        {
                            var costBenefitDto = new CostBenefitDto()
                            {
                                ProjectId = item.ProjectId,
                                Year = new PersianDateTime(item.Year),
                                CostBenefitPeriod = (Enums.CostBenefitPeriod)item.CostBenefitPeriod,
                                ServiceType = (Enums.CostBenefitServiceType)item.ServiceType,
                                ApprovedSales = item.ApprovedSales,
                                RecycleSales = item.RecycleSales,
                                AllCost = item.AllCost,
                                InflationRate = item.InflationRate,
                                GrossProfit = item.GrossProfit
                            };

                            var importResult = await _costBenefitAppService.Create(costBenefitDto);

                        }


                    }
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }


        [DontWrapResult]
        public async Task<ActionResult> CostBenefitRead([DataSourceRequest] DataSourceRequest request)
        {
            var userId = AbpSession.UserId;
            var projects = await _costBenefitAppService.GetAllCostBenefits();

            var result = projects.Select(i => new CostBenefitViewModel
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                ProjectName = i.Project.Title,
                CostBenefitPeriod = i.CostBenefitPeriod,
                ServiceType = i.ServiceType,
                Year = new PersianDateTime(i.Year).Year,
                ApprovedSales = i.ApprovedSales.ToString("N0"),
                RecycleSales = i.RecycleSales.ToString("N0"),
                AllCost = i.AllCost.ToString("N0"),
                InflationRate = i.InflationRate.ToString("N0"),
                GrossProfit = i.GrossProfit.ToString("N0"),
                Profit = (i.RecycleSales + i.ApprovedSales - i.AllCost).ToString("N0")
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
        public async Task<IActionResult> Create(CostBenefitViewModel model)
        {
            var item = new CostBenefitDto()
            {
                ProjectId = model.ProjectId,
                CostBenefitPeriod = model.CostBenefitPeriod,
                ServiceType = model.ServiceType,
                Year = new PersianDateTime(model.Year, 1, 1),
                ApprovedSales = decimal.Parse(model.ApprovedSales),
                RecycleSales = decimal.Parse(model.RecycleSales),
                AllCost = decimal.Parse(model.AllCost),
                GrossProfit = decimal.Parse(model.GrossProfit),
                InflationRate = decimal.Parse(model.InflationRate),
            };
            await _costBenefitAppService.Create(item);
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

            var item = await _costBenefitAppService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));

            var model = new CostBenefitViewModel
            {
                Id = item.Id,
                ProjectId = item.ProjectId,
                CostBenefitPeriod = item.CostBenefitPeriod,
                ServiceType = item.ServiceType,
                Year = new PersianDateTime(item.Year).Year,
                ApprovedSales = item.ApprovedSales.ToString("0."),
                RecycleSales = item.RecycleSales.ToString("0."),
                AllCost = item.AllCost.ToString("0."),
                GrossProfit = item.GrossProfit.ToString("0."),
                InflationRate = item.InflationRate.ToString("0."),
            };

            model.LastGridPage = lastPage;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CostBenefitViewModel model)
        {
            var item = new CostBenefitDto()
            {
                Id = model.Id,
                ProjectId = model.ProjectId,
                ServiceType = model.ServiceType,
                CostBenefitPeriod = model.CostBenefitPeriod,
                Year = new PersianDateTime(model.Year, 1, 1),
                ApprovedSales = decimal.Parse(model.ApprovedSales),
                RecycleSales = decimal.Parse(model.RecycleSales),
                AllCost = decimal.Parse(model.AllCost),
                GrossProfit = decimal.Parse(model.GrossProfit),
                InflationRate = decimal.Parse(model.InflationRate)
            };
            await _costBenefitAppService.Update(item);
            return RedirectToAction("Index", new { lastPage = model.LastGridPage });
        }


        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> CostBenefitDestroy([DataSourceRequest] DataSourceRequest request, CostBenefitViewModel item)
        {
            if (item != null)
            {
                await _costBenefitAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


    }
}