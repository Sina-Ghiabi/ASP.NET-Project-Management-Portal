using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Abp.Web.Models;
using Nexora.PMPortal.Controllers;
using Nexora.PMPortal.Managers;
using Nexora.PMPortal.Managers.Dto;
using Nexora.PMPortal.Web.Startup;
using Nexora.PMPortal.Web.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nexora.PMPortal.Web.Mvc.Controllers
{
    public class ManagerReportController : PMPortalControllerBase
    {
        private readonly IManagerReportAppService _ManagerReportAppService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ManagerReportController(IHostingEnvironment hostingEnvironment, IManagerReportAppService ManagerReportAppService)
        {
            _hostingEnvironment = hostingEnvironment;
            _ManagerReportAppService = ManagerReportAppService;
        }

        public IActionResult Index(int? typeId)
        {
            ViewBag.TypeId = typeId;
            return View();
        }

        #region ManagerReport
        [HttpPost]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> AddManagerReport(ManagerReportViewModel model, IFormFile file)
        {
            try
            {
                if (model != null)
                {
                    if (file != null)
                    {
                        var uniqueFileName = GetUniqueFileName(file.FileName);
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
                        var url = Url.Content("~/uploads/" + uniqueFileName);

                        model.FileUrl = url;
                    }

                    await _ManagerReportAppService.Create(new ManagerReportDto()
                    {
                        Description = model.Description,
                        FileUrl = model.FileUrl,
                        StartDate = PersianDateTime.Parse(model.StartDate),
                        EndDate = PersianDateTime.Parse(model.EndDate),
                        ManagerReportType = model.ManagerReportType
                    });
                    return Json(new { status = "success" });

                }
                return Json(new { status = "error" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [DontWrapResult]
        public async Task<ActionResult> ManagerReportRead([DataSourceRequest] DataSourceRequest request, int id)
        {
            var items = await _ManagerReportAppService.GetManagerReports();
            var reports = items.Where(a => (int)a.ManagerReportType == id).Select(a => new ManagerReportViewModel()
            {
                Id = a.Id,
                Description = a.Description,
                StartDate = new PersianDateTime(a.StartDate).ToShortDateString(),
                EndDate = new PersianDateTime(a.EndDate).ToShortDateString(),
                CreationTime = new PersianDateTime(a.CreationTime).ToShortDateString(),
                FileUrl = a.FileUrl,
            }).ToList();
            var dsResult = reports.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> ManagerReportDestroy([DataSourceRequest] DataSourceRequest request, ManagerReport item)
        {
            if (item != null)
            {
                await _ManagerReportAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 6)
                      + Path.GetExtension(fileName);
        }


        public IActionResult OpenManagerRportForm(int id)
        {
            return ViewComponent("ManagerReportForm", new { managerReportId = id });
        }

        [HttpPost]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> UpdateManagerReport(ManagerReportViewModel model, IFormFile file)
        {
            try
            {
                if (model != null)
                {
                    if (file != null)
                    {
                        var uniqueFileName = GetUniqueFileName(file.FileName);
                        var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
                        var url = Url.Content("~/uploads/" + uniqueFileName);

                        model.FileUrl = url;
                    }
                    var item = _ManagerReportAppService.Get(new EntityDto<int>(model.Id)).Result;

                    item.Description = model.Description;
                    item.FileUrl = model.FileUrl;
                    item.StartDate = PersianDateTime.Parse(model.StartDate);
                    item.EndDate = PersianDateTime.Parse(model.EndDate);

                    await _ManagerReportAppService.Update(item);

                    return Json(new { status = "success" });

                }
                return Json(new { status = "error" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }


    }
}