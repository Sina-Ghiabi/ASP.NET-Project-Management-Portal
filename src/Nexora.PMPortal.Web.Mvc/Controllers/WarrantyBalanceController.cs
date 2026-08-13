using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml;

namespace Nexora.PMPortal.Web.Controllers
{
    [AbpMvcAuthorize]
    public class WarrantyBalanceController : PMPortalControllerBase
    {

        private readonly IProjectAppService _projectAppService;
        private readonly IWarrantyBalanceAppService _WarrantyBalanceAppService;

        public WarrantyBalanceController(IProjectAppService projectAppService, IWarrantyBalanceAppService WarrantyBalanceAppService)
        {
            _projectAppService = projectAppService;
            _WarrantyBalanceAppService = WarrantyBalanceAppService;
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

                    var itemsList = new List<WarrantyBalanceExcelRow>();

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
                                var row = new WarrantyBalanceExcelRow();
                                row.ProjectId = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                                row.Year = new PersianDateTime(Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value), 1, 1);
                                row.ObligationExecution = workSheet.Cells[rowIterator, 3].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString()) : 0;
                                row.PreReceived = workSheet.Cells[rowIterator, 4].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString()) : 0;
                                row.GuaranteeDeduction = workSheet.Cells[rowIterator, 5].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()) : 0;
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
                            var warrantyBalanceDto = new WarrantyBalanceDto()
                            {
                                ProjectId = item.ProjectId,
                                Year = new PersianDateTime(item.Year),
                                ObligationExecution = item.ObligationExecution,
                                PreReceived = item.PreReceived,
                                GuaranteeDeduction = item.GuaranteeDeduction
                            };

                            var importResult = await _WarrantyBalanceAppService.Create(warrantyBalanceDto);

                        }


                    }
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }



        [DontWrapResult]
        public async Task<ActionResult> WarrantyBalanceRead([DataSourceRequest]DataSourceRequest request)
        {
            var userId = AbpSession.UserId;
            var projects = await _WarrantyBalanceAppService.GetAllWarrantyBalances();

            var result = projects.Select(i => new WarrantyBalanceViewModel
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                ProjectName = i.Project.Title,
                Year = new PersianDateTime(i.Year).Year,
                ObligationExecution = i.ObligationExecution.ToString("N0"),
                PreReceived = i.PreReceived.ToString("N0"),
                GuaranteeDeduction = i.GuaranteeDeduction.ToString("N0")
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
        public async Task<IActionResult> Create(WarrantyBalanceViewModel model)
        {
            var item = new WarrantyBalanceDto()
            {
                ProjectId = model.ProjectId,
                Year = new PersianDateTime(model.Year, 1, 1),
                ObligationExecution = decimal.Parse(model.ObligationExecution),
                PreReceived = decimal.Parse(model.PreReceived),
                GuaranteeDeduction = decimal.Parse(model.GuaranteeDeduction),
            };
            await _WarrantyBalanceAppService.Create(item);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id,int lastPage)
        {
            var items = await _projectAppService.GetAllProjects();
            ViewBag.ProjectId = items.Select(a => new ProjectViewModel()
            {
                Id = a.Id,
                Title = a.Title
            }).ToList();

            var item = await _WarrantyBalanceAppService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));

            var model = new WarrantyBalanceViewModel
            {
                Id = item.Id,
                ProjectId = item.ProjectId,
                Year = new PersianDateTime(item.Year).Year,
                ObligationExecution = item.ObligationExecution.ToString("0."),
                PreReceived = item.PreReceived.ToString("0."),
                GuaranteeDeduction = item.GuaranteeDeduction.ToString("0.")
            };

            model.LastGridPage = lastPage;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WarrantyBalanceViewModel model)
        {
            var item = new WarrantyBalanceDto()
            {
                Id = model.Id,
                ProjectId = model.ProjectId,
                Year = new PersianDateTime(model.Year, 1, 1),
                ObligationExecution = decimal.Parse(model.ObligationExecution),
                PreReceived = decimal.Parse(model.PreReceived),
                GuaranteeDeduction = decimal.Parse(model.GuaranteeDeduction),
            };
            await _WarrantyBalanceAppService.Update(item);
            return RedirectToAction("Index", new { lastPage = model.LastGridPage });
        }


        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> WarrantyBalanceDestroy([DataSourceRequest] DataSourceRequest request, WarrantyBalanceViewModel item)
        {
            if (item != null)
            {
                await _WarrantyBalanceAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


    }
}