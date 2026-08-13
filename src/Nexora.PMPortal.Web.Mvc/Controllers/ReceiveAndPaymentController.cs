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
    public class ReceiveAndPaymentController : PMPortalControllerBase
    {

        private readonly IProjectAppService _projectAppService;
        private readonly IReceiveAndPaymentAppService _ReceiveAndPaymentAppService;

        public ReceiveAndPaymentController(IProjectAppService projectAppService, IReceiveAndPaymentAppService ReceiveAndPaymentAppService)
        {
            _projectAppService = projectAppService;
            _ReceiveAndPaymentAppService = ReceiveAndPaymentAppService;
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

                    var itemsList = new List<ReceiveAndPaymentExcelRow>();

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
                                var row = new ReceiveAndPaymentExcelRow();
                                row.ProjectId = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                                row.Year = new PersianDateTime(Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value), 1, 1);
                                row.PreReceived = workSheet.Cells[rowIterator, 3].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString()) : 0;
                                row.Statements = workSheet.Cells[rowIterator, 4].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString()) : 0;
                                row.DepositRelease = workSheet.Cells[rowIterator, 5].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString()) : 0;
                                row.WarrantyReduce = workSheet.Cells[rowIterator, 6].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 6].Value.ToString()) : 0;
                                row.OtherReceipt = workSheet.Cells[rowIterator, 7].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 7].Value.ToString()) : 0;
                                row.IndirectReceipt = workSheet.Cells[rowIterator, 8].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 8].Value.ToString()) : 0;
                                row.OnAccountReceipt = workSheet.Cells[rowIterator, 9].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 9].Value.ToString()) : 0;
                                row.ProjectPayments = workSheet.Cells[rowIterator, 10].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 10].Value.ToString()) : 0;
                                row.WarrantyWithWage = workSheet.Cells[rowIterator, 11].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 11].Value.ToString()) : 0;
                                row.SaleryWithTax = workSheet.Cells[rowIterator, 12].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 12].Value.ToString()) : 0;
                                row.IndirecetPayment = workSheet.Cells[rowIterator, 13].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 13].Value.ToString()) : 0;
                                row.ValueAddedTax = workSheet.Cells[rowIterator, 14].Value != null ? decimal.Parse(workSheet.Cells[rowIterator, 14].Value.ToString()) : 0;

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
                            var receiveAndPaymentDto = new ReceiveAndPaymentDto()
                            {
                                ProjectId = item.ProjectId,
                                Year = item.Year,
                                PreReceived = item.PreReceived,
                                Statements = item.Statements,
                                DepositRelease = item.DepositRelease,
                                WarrantyReduce = item.WarrantyReduce,
                                OtherReceipt = item.OtherReceipt,
                                IndirectReceipt = item.IndirectReceipt,
                                OnAccountReceipt = item.OnAccountReceipt,
                                ProjectPayments = item.ProjectPayments,
                                WarrantyWithWage = item.WarrantyWithWage,
                                SaleryWithTax = item.SaleryWithTax,
                                IndirecetPayment = item.IndirecetPayment,
                                ValueAddedTax = item.ValueAddedTax
                            };

                            var importResult = await _ReceiveAndPaymentAppService.Create(receiveAndPaymentDto);

                        }


                    }
                    return RedirectToAction("Index", "ReceiveAndPayment");
                }
            }
            return RedirectToAction("Index", "ReceiveAndPayment");
        }


        [DontWrapResult]
        public async Task<ActionResult> ReceiveAndPaymentRead([DataSourceRequest] DataSourceRequest request)
        {
            var userId = AbpSession.UserId;
            var projects = await _ReceiveAndPaymentAppService.GetAllItems();

            var result = projects.Select(i => new ReceiveAndPaymentViewModel
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                ProjectName = i.Project.Title,
                Year = new PersianDateTime(i.Year).Year,
                PreReceived = i.PreReceived.ToString("N0"),
                Statements = i.Statements.ToString("N0"),
                DepositRelease = i.DepositRelease.ToString("N0"),
                WarrantyReduce = i.WarrantyReduce.ToString("N0"),
                OtherReceipt = i.OtherReceipt.ToString("N0"),
                IndirectReceipt = i.IndirectReceipt.ToString("N0"),
                OnAccountReceipt = i.OnAccountReceipt.ToString("N0"),
                ProjectPayments = i.ProjectPayments.ToString("N0"),
                WarrantyWithWage = i.WarrantyWithWage.ToString("N0"),
                SaleryWithTax = i.SaleryWithTax.ToString("N0"),
                IndirecetPayment = i.IndirecetPayment.ToString("N0"),
                ValueAddedTax = i.ValueAddedTax.ToString("N0"),
                TotalReceive = (i.PreReceived + i.Statements + i.DepositRelease + i.WarrantyReduce + i.OtherReceipt + i.IndirectReceipt + i.OnAccountReceipt).ToString("N0"),
                TotalPayment = (i.ProjectPayments + i.WarrantyWithWage + i.SaleryWithTax + i.IndirecetPayment + i.ValueAddedTax).ToString("N0")
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
        public async Task<IActionResult> Create(ReceiveAndPaymentViewModel model)
        {
            var item = new ReceiveAndPaymentDto()
            {
                ProjectId = model.ProjectId,
                Year = new PersianDateTime(model.Year, 1, 1),
                PreReceived = decimal.Parse(model.PreReceived),
                Statements = decimal.Parse(model.Statements),
                DepositRelease = decimal.Parse(model.DepositRelease),
                WarrantyReduce = decimal.Parse(model.WarrantyReduce),
                OtherReceipt = decimal.Parse(model.OtherReceipt),
                IndirectReceipt = decimal.Parse(model.IndirectReceipt),
                OnAccountReceipt = decimal.Parse(model.OnAccountReceipt),
                ProjectPayments = decimal.Parse(model.ProjectPayments),
                WarrantyWithWage = decimal.Parse(model.WarrantyWithWage),
                SaleryWithTax = decimal.Parse(model.SaleryWithTax),
                IndirecetPayment = decimal.Parse(model.IndirecetPayment),
                ValueAddedTax = decimal.Parse(model.ValueAddedTax)
            };
            await _ReceiveAndPaymentAppService.Create(item);
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

            var item = await _ReceiveAndPaymentAppService.Get(new Abp.Application.Services.Dto.EntityDto<int>(id));

            var model = new ReceiveAndPaymentViewModel
            {
                Id = item.Id,
                ProjectId = item.ProjectId,
                Year = new PersianDateTime(item.Year).Year,
                ProjectName = item.Project.Title,
                PreReceived = item.PreReceived.ToString("0."),
                Statements = item.Statements.ToString("0."),
                DepositRelease = item.DepositRelease.ToString("0."),
                WarrantyReduce = item.WarrantyReduce.ToString("0."),
                OtherReceipt = item.OtherReceipt.ToString("0."),
                IndirectReceipt = item.IndirectReceipt.ToString("0."),
                OnAccountReceipt = item.OnAccountReceipt.ToString("0."),
                ProjectPayments = item.ProjectPayments.ToString("0."),
                WarrantyWithWage = item.WarrantyWithWage.ToString("0."),
                SaleryWithTax = item.SaleryWithTax.ToString("0."),
                IndirecetPayment = item.IndirecetPayment.ToString("0."),
                ValueAddedTax = item.ValueAddedTax.ToString("0.")
            };

            model.LastGridPage = lastPage;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReceiveAndPaymentViewModel model)
        {
            var item = new ReceiveAndPaymentDto()
            {
                Id = model.Id,
                ProjectId = model.ProjectId,
                Year = new PersianDateTime(model.Year, 1, 1),
                PreReceived = decimal.Parse(model.PreReceived),
                Statements = decimal.Parse(model.Statements),
                DepositRelease = decimal.Parse(model.DepositRelease),
                WarrantyReduce = decimal.Parse(model.WarrantyReduce),
                OtherReceipt = decimal.Parse(model.OtherReceipt),
                IndirectReceipt = decimal.Parse(model.IndirectReceipt),
                OnAccountReceipt = decimal.Parse(model.OnAccountReceipt),
                ProjectPayments = decimal.Parse(model.ProjectPayments),
                WarrantyWithWage = decimal.Parse(model.WarrantyWithWage),
                SaleryWithTax = decimal.Parse(model.SaleryWithTax),
                IndirecetPayment = decimal.Parse(model.IndirecetPayment),
                ValueAddedTax = decimal.Parse(model.ValueAddedTax)
            };

            await _ReceiveAndPaymentAppService.Update(item);
            return RedirectToAction("Index", new { lastPage = model.LastGridPage });
        }


        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> ReceiveAndPaymentDestroy([DataSourceRequest] DataSourceRequest request, ReceiveAndPaymentViewModel item)
        {
            if (item != null)
            {
                await _ReceiveAndPaymentAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


    }
}