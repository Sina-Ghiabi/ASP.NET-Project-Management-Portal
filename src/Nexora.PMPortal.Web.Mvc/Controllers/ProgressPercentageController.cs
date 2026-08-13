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
    public class ProgressPercentageController : PMPortalControllerBase
    {

        private readonly IProjectAppService _projectAppService;
        private readonly IProgressPercentageAppService _ProgressPercentageAppService;
        private readonly IProject_User_MappingAppService _project_User_MappingAppService;

        public ProgressPercentageController(IProjectAppService projectAppService, IProgressPercentageAppService ProgressPercentageAppService, IProject_User_MappingAppService project_User_MappingAppService)
        {
            _projectAppService = projectAppService;
            _ProgressPercentageAppService = ProgressPercentageAppService;
            _project_User_MappingAppService = project_User_MappingAppService;
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

                    var itemsList = new List<ProgressPercentageRow>();

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
                                var row = new ProgressPercentageRow();
                                row.ProjectId = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                                row.Year = (Enums.SelectiveYear)Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value);
                                row.Month = (Enums.SelectiveMonth)Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value);

                                row.EngineeringProgressPlan = workSheet.Cells[rowIterator, 4].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString()) : 0;
                                row.EngineeringProgressReal = workSheet.Cells[rowIterator, 5].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()) : 0;

                                row.SupplyProgressPlan = workSheet.Cells[rowIterator, 6].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 6].Value.ToString()) : 0;
                                row.SupplyProgressReal = workSheet.Cells[rowIterator, 7].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 7].Value.ToString()) : 0;

                                row.ExecutionProgressPlan = workSheet.Cells[rowIterator, 8].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 8].Value.ToString()) : 0;
                                row.ExecutionProgressReal = workSheet.Cells[rowIterator, 9].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 9].Value.ToString()) : 0;


                                row.TotalProgressPlan = workSheet.Cells[rowIterator, 10].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 10].Value.ToString()) : 0;
                                row.TotalProgressReal = workSheet.Cells[rowIterator, 11].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 11].Value.ToString()) : 0;

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
                            var progressPercentageDto = new ProgressPercentageDto()
                            {
                                ProjectId = item.ProjectId,
                                Year = item.Year,
                                Month = item.Month,
                                EngineeringProgressPlan = item.EngineeringProgressPlan,
                                EngineeringProgressReal = item.EngineeringProgressReal,
                                SupplyProgressPlan = item.SupplyProgressPlan,
                                SupplyProgressReal = item.SupplyProgressReal,
                                ExecutionProgressPlan = item.ExecutionProgressPlan,
                                ExecutionProgressReal = item.ExecutionProgressReal,
                                TotalExecutionProgressPlan = item.TotalExecutionProgressPlan,
                                TotalExecutionProgressReal = item.TotalExecutionProgressReal,
                                TotalProgressPlan = item.TotalProgressPlan,
                                TotalProgressReal = item.TotalProgressReal
                            };

                            var importResult = await _ProgressPercentageAppService.Create(progressPercentageDto);

                        }


                    }
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }



        [DontWrapResult]
        public async Task<ActionResult> ProgressPercentageRead([DataSourceRequest] DataSourceRequest request)
        {
            var userId = AbpSession.UserId;
            List<ProgressPercentageViewModel> List = new List<ProgressPercentageViewModel>();
            var UsersProjectsIDs = _project_User_MappingAppService.GetUserProjects(userId.Value).Result.Select(a => a.ProjectId);

            foreach (var Item in UsersProjectsIDs)
            {
                var projects = await _ProgressPercentageAppService.GetProjectProgressPercentages(Item);
                List.AddRange(projects.Select(i => new ProgressPercentageViewModel
                {
                    Id = i.Id,
                    ProjectId = i.ProjectId,

                    ProjectName = i.Project.Title,
                    Year = i.Year,
                    Month = i.Month,
                    EngineeringProgressPlan = i.EngineeringProgressPlan.ToString("0.00"),
                    EngineeringProgressReal = i.EngineeringProgressReal.ToString("0.00"),

                    SupplyProgressPlan = i.SupplyProgressPlan.ToString("0.00"),
                    SupplyProgressReal = i.SupplyProgressReal.ToString("0.00"),

                    ExecutionProgressPlan = i.ExecutionProgressPlan.ToString("0.00"),
                    ExecutionProgressReal = i.ExecutionProgressReal.ToString("0.00"),

                    TotalExecutionProgressPlan = i.TotalExecutionProgressPlan.ToString(),
                    TotalExecutionProgressReal = i.TotalExecutionProgressReal.ToString(),

                    TotalProgressPlan = i.TotalProgressPlan.ToString("0.00"),
                    TotalProgressReal = i.TotalProgressReal.ToString("0.00")
                }));
            }

            var dsResult = List.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });

        }

        public async Task<IActionResult> Create()
        {
            var items = await _project_User_MappingAppService.GetUserProjects(AbpSession.UserId.Value);

            ViewBag.ProjectId = items.Select(a => new ProjectViewModel()
            {
                Id = a.ProjectId,
                Title = a.Project.Title
            }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProgressPercentageViewModel model)
        {
            var item = new ProgressPercentageDto()
            {
                ProjectId = model.ProjectId,

                Year = model.Year,
                Month = model.Month,
                EngineeringProgressPlan = decimal.Parse(model.EngineeringProgressPlan),
                EngineeringProgressReal = decimal.Parse(model.EngineeringProgressReal),

                SupplyProgressPlan = decimal.Parse(model.SupplyProgressPlan),
                SupplyProgressReal = decimal.Parse(model.SupplyProgressReal),

                ExecutionProgressPlan = decimal.Parse(model.ExecutionProgressPlan),
                ExecutionProgressReal = decimal.Parse(model.ExecutionProgressReal),

                TotalExecutionProgressPlan = decimal.Parse(model.TotalExecutionProgressPlan),
                TotalExecutionProgressReal = decimal.Parse(model.TotalExecutionProgressReal),

                TotalProgressPlan = decimal.Parse(model.TotalProgressPlan),
                TotalProgressReal = decimal.Parse(model.TotalProgressReal),
            };
            await _ProgressPercentageAppService.Create(item);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id, int lastPage)
        {
            var items = await _project_User_MappingAppService.GetUserProjects(AbpSession.UserId.Value);
            ViewBag.ProjectId = items.Select(a => new ProjectViewModel()
            {
                Id = a.ProjectId,
                Title = a.Project.Title
            }).ToList();

            var item = await _ProgressPercentageAppService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));

            var model = new ProgressPercentageViewModel
            {
                Id = item.Id,

                ProjectId = item.ProjectId,
                Year = item.Year,
                Month = item.Month,
                EngineeringProgressPlan = item.EngineeringProgressPlan.ToString("0.00"),
                EngineeringProgressReal = item.EngineeringProgressReal.ToString("0.00"),

                SupplyProgressPlan = item.SupplyProgressPlan.ToString("0.00"),
                SupplyProgressReal = item.SupplyProgressReal.ToString("0.00"),

                ExecutionProgressPlan = item.ExecutionProgressPlan.ToString("0.00"),
                ExecutionProgressReal = item.ExecutionProgressReal.ToString("0.00"),

                TotalExecutionProgressPlan = item.TotalExecutionProgressPlan.ToString(),
                TotalExecutionProgressReal = item.TotalExecutionProgressReal.ToString(),

                TotalProgressPlan = item.TotalProgressPlan.ToString("0.00"),
                TotalProgressReal = item.TotalProgressReal.ToString("0.00")
            };

            model.LastGridPage = lastPage;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProgressPercentageViewModel model)
        {
            var item = new ProgressPercentageDto()
            {
                Id = model.Id,
                ProjectId = model.ProjectId,

                Year = model.Year,
                Month = model.Month,
                EngineeringProgressPlan = decimal.Parse(model.EngineeringProgressPlan),
                EngineeringProgressReal = decimal.Parse(model.EngineeringProgressReal),

                SupplyProgressPlan = decimal.Parse(model.SupplyProgressPlan),
                SupplyProgressReal = decimal.Parse(model.SupplyProgressReal),

                ExecutionProgressPlan = decimal.Parse(model.ExecutionProgressPlan),
                ExecutionProgressReal = decimal.Parse(model.ExecutionProgressReal),

                TotalExecutionProgressPlan = decimal.Parse(model.TotalExecutionProgressPlan),
                TotalExecutionProgressReal = decimal.Parse(model.TotalExecutionProgressReal),

                TotalProgressPlan = decimal.Parse(model.TotalProgressPlan),
                TotalProgressReal = decimal.Parse(model.TotalProgressReal)
            };
            await _ProgressPercentageAppService.Update(item);
            return RedirectToAction("Index", new { lastPage = model.LastGridPage });
        }


        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> ProgressPercentageDestroy([DataSourceRequest] DataSourceRequest request, ProgressPercentageViewModel item)
        {
            if (item != null)
            {
                await _ProgressPercentageAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


    }
}