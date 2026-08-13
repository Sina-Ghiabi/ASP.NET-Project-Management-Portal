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
    public class LaborInventoryController : PMPortalControllerBase
    {

        private readonly IProjectAppService _projectAppService;
        private readonly ILaborInventoryAppService _LaborInventoryAppService;

        public LaborInventoryController(IProjectAppService projectAppService, ILaborInventoryAppService LaborInventoryAppService)
        {
            _projectAppService = projectAppService;
            _LaborInventoryAppService = LaborInventoryAppService;
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

                    var itemsList = new List<LaborInventoryExcelRow>();

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
                                var row = new LaborInventoryExcelRow();
                                row.ProjectId = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                                row.Year = new PersianDateTime(Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value), 1, 1);
                                row.Month1_Amount = workSheet.Cells[rowIterator, 3].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString()) : 0;
                                row.Month2_Amount = workSheet.Cells[rowIterator, 4].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString()) : 0;
                                row.Month3_Amount = workSheet.Cells[rowIterator, 5].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()) : 0;
                                row.Month4_Amount = workSheet.Cells[rowIterator, 6].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 6].Value.ToString()) : 0;
                                row.Month5_Amount = workSheet.Cells[rowIterator, 7].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 7].Value.ToString()) : 0;
                                row.Month6_Amount = workSheet.Cells[rowIterator, 8].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 8].Value.ToString()) : 0;
                                row.Month7_Amount = workSheet.Cells[rowIterator, 9].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 9].Value.ToString()) : 0;
                                row.Month8_Amount = workSheet.Cells[rowIterator, 10].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 10].Value.ToString()) : 0;
                                row.Month9_Amount = workSheet.Cells[rowIterator, 11].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 11].Value.ToString()) : 0;
                                row.Month10_Amount = workSheet.Cells[rowIterator, 12].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 12].Value.ToString()) : 0;
                                row.Month11_Amount = workSheet.Cells[rowIterator, 13].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 13].Value.ToString()) : 0;
                                row.Month12_Amount = workSheet.Cells[rowIterator, 14].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 14].Value.ToString()) : 0;
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
                            var laborInventoryDto = new LaborInventoryDto()
                            {
                                ProjectId = item.ProjectId,
                                Year = new PersianDateTime(item.Year),
                                Month1_Amount = item.Month1_Amount,
                                Month2_Amount = item.Month2_Amount,
                                Month3_Amount = item.Month3_Amount,
                                Month4_Amount = item.Month4_Amount,
                                Month5_Amount = item.Month5_Amount,
                                Month6_Amount = item.Month6_Amount,
                                Month7_Amount = item.Month7_Amount,
                                Month8_Amount = item.Month8_Amount,
                                Month9_Amount = item.Month9_Amount,
                                Month10_Amount = item.Month10_Amount,
                                Month11_Amount = item.Month11_Amount,
                                Month12_Amount = item.Month12_Amount
                            };

                            var importResult = await _LaborInventoryAppService.Create(laborInventoryDto);

                        }


                    }
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }



        [DontWrapResult]
        public async Task<ActionResult> LaborInventoryRead([DataSourceRequest] DataSourceRequest request)
        {
            var userId = AbpSession.UserId;
            var projects = await _LaborInventoryAppService.GetAllLaborInventories();

            var result = projects.Select(i => new LaborInventoryViewModel
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                ProjectName = i.Project.Title,
                Year = new PersianDateTime(i.Year).Year,
                Month1_Amount = i.Month1_Amount,
                Month2_Amount = i.Month2_Amount,
                Month3_Amount = i.Month3_Amount,
                Month4_Amount = i.Month4_Amount,
                Month5_Amount = i.Month5_Amount,
                Month6_Amount = i.Month6_Amount,
                Month7_Amount = i.Month7_Amount,
                Month8_Amount = i.Month8_Amount,
                Month9_Amount = i.Month9_Amount,
                Month10_Amount = i.Month10_Amount,
                Month11_Amount = i.Month11_Amount,
                Month12_Amount = i.Month12_Amount
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
        public async Task<IActionResult> Create(LaborInventoryViewModel model)
        {
            var item = new LaborInventoryDto()
            {
                ProjectId = model.ProjectId,
                Year = new PersianDateTime(model.Year, 1, 1),
                Month1_Amount = model.Month1_Amount,
                Month2_Amount = model.Month2_Amount,
                Month3_Amount = model.Month3_Amount,
                Month4_Amount = model.Month4_Amount,
                Month5_Amount = model.Month5_Amount,
                Month6_Amount = model.Month6_Amount,
                Month7_Amount = model.Month7_Amount,
                Month8_Amount = model.Month8_Amount,
                Month9_Amount = model.Month9_Amount,
                Month10_Amount = model.Month10_Amount,
                Month11_Amount = model.Month11_Amount,
                Month12_Amount = model.Month12_Amount
            };
            await _LaborInventoryAppService.Create(item);
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

            var item = await _LaborInventoryAppService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));

            var model = new LaborInventoryViewModel
            {
                Id = item.Id,
                ProjectId = item.ProjectId,
                Year = new PersianDateTime(item.Year).Year,
                Month1_Amount = item.Month1_Amount,
                Month2_Amount = item.Month2_Amount,
                Month3_Amount = item.Month3_Amount,
                Month4_Amount = item.Month4_Amount,
                Month5_Amount = item.Month5_Amount,
                Month6_Amount = item.Month6_Amount,
                Month7_Amount = item.Month7_Amount,
                Month8_Amount = item.Month8_Amount,
                Month9_Amount = item.Month9_Amount,
                Month10_Amount = item.Month10_Amount,
                Month11_Amount = item.Month11_Amount,
                Month12_Amount = item.Month12_Amount
            };

            model.LastGridPage = lastPage;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LaborInventoryViewModel model)
        {
            var item = new LaborInventoryDto()
            {
                Id = model.Id,
                ProjectId = model.ProjectId,
                Year = new PersianDateTime(model.Year, 1, 1),
                Month1_Amount = model.Month1_Amount,
                Month2_Amount = model.Month2_Amount,
                Month3_Amount = model.Month3_Amount,
                Month4_Amount = model.Month4_Amount,
                Month5_Amount = model.Month5_Amount,
                Month6_Amount = model.Month6_Amount,
                Month7_Amount = model.Month7_Amount,
                Month8_Amount = model.Month8_Amount,
                Month9_Amount = model.Month9_Amount,
                Month10_Amount = model.Month10_Amount,
                Month11_Amount = model.Month11_Amount,
                Month12_Amount = model.Month12_Amount
            };
            await _LaborInventoryAppService.Update(item);
            return RedirectToAction("Index", new { lastPage = model.LastGridPage });
        }


        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> LaborInventoryDestroy([DataSourceRequest] DataSourceRequest request, LaborInventoryViewModel item)
        {
            if (item != null)
            {
                await _LaborInventoryAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


    }
}