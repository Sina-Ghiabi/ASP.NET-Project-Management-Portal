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
using Nexora.PMPortal.Projects;
using Nexora.PMPortal.Projects.Dto;
using Nexora.PMPortal.Web.Startup;
using Nexora.PMPortal.Web.ViewModels;
using Nexora.PMPortal.Web.Views.Shared.Components.FinancialStatementForm;
using Nexora.PMPortal.Web.Views.Shared.Components.ProjectReportForm;
using Nexora.PMPortal.Web.Views.Shared.Components.ProjectPlanForm;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MD.PersianDateTime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data.SqlClient;

namespace Nexora.PMPortal.Web.Mvc.Controllers
{
    [AbpMvcAuthorize]
    public class ProjectController : PMPortalControllerBase
    {
        private readonly IProjectAppService _projectAppService;
        private readonly IProjectReportAppService _projectReportAppService;
        private readonly IProjectPlanAppService _projectPlanAppService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFinancialStatementAppService _financialStatementAppService;
        private readonly IProjectMediaAppService _projectMediaAppService;
        private readonly IProject_User_MappingAppService _project_User_MappingAppService;
        private readonly IConfiguration _configuration;

        public ProjectController(IProjectAppService projectAppService, IFinancialStatementAppService financialStatementAppService,
            IHostingEnvironment hostingEnvironment,
            IProjectReportAppService projectReportAppService,
            IProjectPlanAppService projectPlanAppService,
            IProjectMediaAppService projectMediaAppService,
            IProject_User_MappingAppService project_User_MappingAppService,
            IConfiguration configuration)
        {
            _projectAppService = projectAppService;
            _hostingEnvironment = hostingEnvironment;
            _financialStatementAppService = financialStatementAppService;
            _projectReportAppService = projectReportAppService;
            _projectPlanAppService = projectPlanAppService;
            _projectMediaAppService = projectMediaAppService;
            _project_User_MappingAppService = project_User_MappingAppService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }


        [DontWrapResult]
        public async Task<ActionResult> ProjectRead([DataSourceRequest] DataSourceRequest request)
        {
            var userId = AbpSession.UserId;
            var projects = await _project_User_MappingAppService.GetUserProjects(userId.Value);

            //var items = await _projectAppService.GetAllProjects();
            var result = projects.Select(i => new ProjectViewModel
            {
                Id = i.ProjectId,
                Title = i.Project.Title,
                ProjectStatus = i.Project.ProjectStatus,
                ProjectType = i.Project.ProjectType,
                FriendlyCreationTime = new PersianDateTime(i.Project.CreationTime).ToShortDateString()
            }).ToList();

            var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [DisableValidation]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            var project = new ProjectDto()
            {
                Title = model.Title,
                Code = model.Code,
                ProjectType = model.ProjectType,
                ProjectStatus = model.ProjectStatus,
                Client = model.Client,
                PlannigManager = model.PlannigManager,
                Consultant = model.Consultant,
                ProjectManager = model.ProjectManager,
                Partners = model.Partners,
                PartnerShares = model.PartnerShares,
                AmountCurrencyType = model.AmountCurrencyType,
                InitialContractAmount = model.InitialContractAmount != null ? decimal.Parse(model.InitialContractAmount) : default(decimal?),
                ExtensionContractAmount = model.ExtensionContractAmount != null ? decimal.Parse(model.ExtensionContractAmount) : default(decimal?),
                InitialContractAmountInCurrency = model.InitialContractAmountInCurrency != null ? decimal.Parse(model.InitialContractAmountInCurrency) : default(decimal?),
                ExtensionContractAmountInCurrency = model.ExtensionContractAmountInCurrency != null ? decimal.Parse(model.ExtensionContractAmountInCurrency) : default(decimal?),
                StartDate = model.StartDate != null ? PersianDateTime.Parse(model.StartDate) : default(DateTime?),
                TemporaryDeliveryDate = model.TemporaryDeliveryDate != null ? PersianDateTime.Parse(model.TemporaryDeliveryDate) : default(DateTime?),
                FinalDeliveryDate = model.FinalDeliveryDate != null ? PersianDateTime.Parse(model.FinalDeliveryDate) : default(DateTime?),
                ProjectContractPeriod = model.ProjectContractPeriod,
                ProjectCompletionDate = model.ProjectCompletionDate != null ? PersianDateTime.Parse(model.ProjectCompletionDate) : default(DateTime?),
                ContractEndDate = model.ContractEndDate != null ? PersianDateTime.Parse(model.ContractEndDate) : default(DateTime?),
                Description = model.Description,
                IsActive = true,
                ShowInUsageReport = model.ShowInUsageReport,
                UsageReportDisplayOrder = model.UsageReportDisplayOrder
            };
            await _projectAppService.Create(project);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _projectAppService.Get(new EntityDto<int>(id));
            var model = new ProjectViewModel(item);
            return View(model);
        }


        [HttpPost]
        [DisableValidation]
        public async Task<IActionResult> Edit(ProjectViewModel model)
        {
            var item = await _projectAppService.Get(new EntityDto<int>(model.Id));

            item.Title = model.Title;
            item.Code = model.Code;
            item.ProjectType = model.ProjectType;
            item.ProjectStatus = model.ProjectStatus;
            item.Client = model.Client;
            item.PlannigManager = model.PlannigManager;
            item.Consultant = model.Consultant;
            item.ProjectManager = model.ProjectManager;
            item.Partners = model.Partners;
            item.ProjectSite = model.ProjectSite;
            item.PartnerShares = model.PartnerShares;
            item.AmountCurrencyType = model.AmountCurrencyType;
            item.InitialContractAmount = model.InitialContractAmount != null ? decimal.Parse(model.InitialContractAmount) : default(decimal?);
            item.ExtensionContractAmount = model.ExtensionContractAmount != null ? decimal.Parse(model.ExtensionContractAmount) : default(decimal?);
            item.InitialContractAmountInCurrency = model.InitialContractAmountInCurrency != null ? decimal.Parse(model.InitialContractAmountInCurrency) : default(decimal?);
            item.ExtensionContractAmountInCurrency = model.ExtensionContractAmountInCurrency != null ? decimal.Parse(model.ExtensionContractAmountInCurrency) : default(decimal?);
            item.StartDate = model.StartDate != null ? PersianDateTime.Parse(model.StartDate) : default(DateTime?);
            item.TemporaryDeliveryDate = model.TemporaryDeliveryDate != null ? PersianDateTime.Parse(model.TemporaryDeliveryDate) : default(DateTime?);
            item.FinalDeliveryDate = model.FinalDeliveryDate != null ? PersianDateTime.Parse(model.FinalDeliveryDate) : default(DateTime?);
            item.ProjectContractPeriod = model.ProjectContractPeriod;
            item.ProjectCompletionDate = model.ProjectCompletionDate != null ? PersianDateTime.Parse(model.ProjectCompletionDate) : default(DateTime?);
            item.ContractEndDate = model.ContractEndDate != null ? PersianDateTime.Parse(model.ContractEndDate) : default(DateTime?);
            item.Description = model.Description;
            item.ShowInUsageReport = model.ShowInUsageReport;
            item.UsageReportDisplayOrder = model.UsageReportDisplayOrder;

            await _projectAppService.Update(item);
            return RedirectToAction("Index");
        }

        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> ProjectDestroy([DataSourceRequest] DataSourceRequest request, ProjectViewModel item)
        {
            if (item != null)
            {
                await _projectAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _projectAppService.Get(new EntityDto<int>(id));
            var model = new ProjectViewModel(item);

            var medias = await _projectMediaAppService.GetProjectMedias(id);
            model.ProjectMedias = medias.OrderByDescending(a => a.SubmitDate).Select(a => new ProjectMediaViewModel()
            {
                Id = a.Id,
                Title = a.Title,
                ProjectId = a.ProjectId,
                SubmitDate = new PersianDateTime(a.SubmitDate).ToShortDateString(),
                ImageUrl = a.ImageUrl
            }).ToList();

            model.FinancialStatementForm = new FinancialStatementViewModel()
            {
                ProjectId = id
            };
            model.ProjectReportForm = new ProjectReportViewModel()
            {
                ProjectId = id
            };
            model.ProjectMediaForm = new ProjectMediaViewModel()
            {
                ProjectId = id
            };

            var statements = await _financialStatementAppService.GetProjectFinancialStatements(id);

            model.LastAdjustmentFinancialStatemntValue = statements.OrderByDescending(a => a.Id).FirstOrDefault(a => a.FinancialStatementType == Enums.FinancialStatementType.Adjustment)?.TotalAmount.Value.ToString("N0");

            model.LastFinalFinancialStatemntValue = statements.OrderByDescending(a => a.Id).FirstOrDefault(a => a.FinancialStatementType == Enums.FinancialStatementType.Final)?.TotalAmount.Value.ToString("N0");

            model.LastTemporaryFinancialStatemntValue = statements.OrderByDescending(a => a.Id).FirstOrDefault(a => a.FinancialStatementType == Enums.FinancialStatementType.Temporary)?.TotalAmount.Value.ToString("N0");

            return View(model);
        }

        [DontWrapResult]
        public IActionResult ProjectMediaAjax(int ID, int Start, int End)
        {
            var List = new List<ProjectMediaViewModel>();

            SqlConnection SQLConnection = new SqlConnection(_configuration.GetConnectionString("Default"));
            var Query = "SELECT RowNumber , Id , Title , ProjectId , SubmitDate , ImageUrl FROM (SELECT ROW_NUMBER() OVER (order by SubmitDate Desc) AS RowNumber , * FROM ProjectMedias WHERE ProjectId =" + ID + " AND IsDeleted = 'False') T WHERE RowNumber >=" + Start + " and RowNumber <" + End + "";
            SqlCommand Command = new SqlCommand(Query, SQLConnection);
            SQLConnection.Open();
            var Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                List.Add(new ProjectMediaViewModel { Id = (int)Reader[1], Title = Reader[2].ToString(), ProjectId = (int)Reader[3], SubmitDate = new PersianDateTime((DateTime)Reader[4]).ToShortDateString() , ImageUrl = Reader[5].ToString() });
            }

            return Json(List.ToList(), new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        #region ProjectFinancialStatement
        [HttpPost]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> AddOrUpdateFinancialStatement(FinancialStatementFormViewModel model, IFormFile file)
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
                    if (model.Id == null)
                    {
                        await _financialStatementAppService.Create(new FinancialStatementDto()
                        {
                            FinancialStatementType = model.FinancialStatementType,
                            CurrencyType = model.CurrencyType,
                            Number = model.Number,
                            Period = model.Period,
                            TotalAmount = model.TotalAmount,
                            PeriodicAmount = model.PeriodicAmount,
                            FileUrl = model.FileUrl,
                            StartDate = model.StartDate != null ? PersianDateTime.Parse(model.StartDate) : default(DateTime?),
                            EndDate = model.EndDate != null ? PersianDateTime.Parse(model.EndDate) : default(DateTime?),
                            ProjectId = model.ProjectId,
                        });
                    }
                    else
                    {
                        await _financialStatementAppService.Update(new FinancialStatementDto()
                        {
                            Id = model.Id.Value,
                            FinancialStatementType = model.FinancialStatementType,
                            CurrencyType = model.CurrencyType,
                            Number = model.Number,
                            Period = model.Period,
                            TotalAmount = model.TotalAmount,
                            PeriodicAmount = model.PeriodicAmount,
                            FileUrl = model.FileUrl,
                            StartDate = model.StartDate != null ? PersianDateTime.Parse(model.StartDate) : default(DateTime?),
                            EndDate = model.EndDate != null ? PersianDateTime.Parse(model.EndDate) : default(DateTime?),
                            ProjectId = model.ProjectId,
                        });
                    }

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
        public async Task<ActionResult> FinancialStatementRead([DataSourceRequest] DataSourceRequest request, int projectId)
        {
            var list = await _financialStatementAppService.GetProjectFinancialStatements(projectId);
            var items = list.Select(a => new FinancialStatementViewModel()
            {
                Id = a.Id,
                FinancialStatementType = a.FinancialStatementType,
                CurrencyType = a.CurrencyType,
                Number = a.Number,
                Period = a.Period,
                TotalAmount = a.TotalAmount != null ? a.TotalAmount.Value.ToString("N0") : "-",
                PeriodicAmount = a.PeriodicAmount != null ? a.PeriodicAmount.Value.ToString("N0") : "-",
                FileUrl = a.FileUrl,
                StartDate = a.StartDate != null ? new PersianDateTime(a.StartDate).ToShortDateString() : "-",
                EndDate = a.EndDate != null ? new PersianDateTime(a.EndDate).ToShortDateString() : "-",
                ProjectId = a.ProjectId
            }).ToList();
            var dsResult = items.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> FinancialStatementDestroy([DataSourceRequest] DataSourceRequest request, FinancialStatementViewModel item)
        {
            if (item != null)
            {
                await _financialStatementAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }

        public IActionResult OpenFinancialStatementForm(int? id, int projectId)
        {
            return ViewComponent("FinancialStatementForm", new { statementId = id, projectId });
        }

        #endregion

        #region ProjectReport
        [HttpPost]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> AddOrUpdateProjectReport(ProjectReportFormViewModel model, IFormFile file)
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
                    if (model.Id == null)
                    {
                        await _projectReportAppService.Create(new ProjectReportDto()
                        {
                            Description = model.Description,
                            FileUrl = model.FileUrl,
                            ProjectReportType = model.ProjectReportType,
                            StartDate = PersianDateTime.Parse(model.StartDate),
                            EndDate = PersianDateTime.Parse(model.EndDate),
                            ProjectId = model.ProjectId,
                        });
                    }
                    else
                    {
                        await _projectReportAppService.Update(new ProjectReportDto()
                        {
                            Id = model.Id.Value,
                            Description = model.Description,
                            FileUrl = model.FileUrl,
                            ProjectReportType = model.ProjectReportType,
                            StartDate = PersianDateTime.Parse(model.StartDate),
                            EndDate = PersianDateTime.Parse(model.EndDate),
                            ProjectId = model.ProjectId,
                        });
                    }

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
        public async Task<ActionResult> ProjectReportRead([DataSourceRequest] DataSourceRequest request, int projectId, int? type)
        {
            var items = await _projectReportAppService.GetProjectReports(projectId);
            var reports = items.Select(a => new ProjectReportViewModel()
            {
                Id = a.Id,
                Description = a.Description,
                ProjectId = a.ProjectId,
                StartDate = new PersianDateTime(a.StartDate).ToShortDateString(),
                EndDate = new PersianDateTime(a.EndDate).ToShortDateString(),
                FileUrl = a.FileUrl,
                ProjectReportType = a.ProjectReportType
            }).ToList();
            if (type != null)
            {
                reports = reports.Where(a => (int)a.ProjectReportType == type.Value).ToList();
            }
            var dsResult = reports.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> ProjectReportDestroy([DataSourceRequest] DataSourceRequest request, ProjectReport item)
        {
            if (item != null)
            {
                await _projectReportAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


        public IActionResult OpenProjectReportForm(int? id, int projectId)
        {
            return ViewComponent("ProjectReportForm", new { reportId = id, projectId });
        }



        #region ProjectPlan
        [HttpPost]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> AddOrUpdateProjectPlan(ProjectPlanFormViewModel model, IFormFile file)
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
                    if (model.Id == null)
                    {
                        await _projectPlanAppService.Create(new ProjectPlanDto()
                        {
                            Description = model.Description,
                            FileUrl = model.FileUrl,
                            ProjectPlanType = model.ProjectPlanType,
                            StartDate = PersianDateTime.Parse(model.StartDate),
                            EndDate = PersianDateTime.Parse(model.EndDate),
                            ProjectId = model.ProjectId,
                        });
                    }
                    else
                    {
                        await _projectPlanAppService.Update(new ProjectPlanDto()
                        {
                            Id = model.Id.Value,
                            Description = model.Description,
                            FileUrl = model.FileUrl,
                            ProjectPlanType = model.ProjectPlanType,
                            StartDate = PersianDateTime.Parse(model.StartDate),
                            EndDate = PersianDateTime.Parse(model.EndDate),
                            ProjectId = model.ProjectId,
                        });
                    }

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
        public async Task<ActionResult> ProjectPlanRead([DataSourceRequest] DataSourceRequest request, int projectId, int? type)
        {
            var items = await _projectPlanAppService.GetProjectPlan(projectId);
            var reports = items.Select(a => new ProjectPlanViewModel()
            {
                Id = a.Id,
                Description = a.Description,
                ProjectId = a.ProjectId,
                StartDate = new PersianDateTime(a.StartDate).ToShortDateString(),
                EndDate = new PersianDateTime(a.EndDate).ToShortDateString(),
                FileUrl = a.FileUrl,
                ProjectPlanType = a.ProjectPlanType
            }).ToList();
            if (type != null)
            {
                reports = reports.Where(a => (int)a.ProjectPlanType == type.Value).ToList();
            }
            var dsResult = reports.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> ProjectPlanDestroy([DataSourceRequest] DataSourceRequest request, ProjectPlan item)
        {
            if (item != null)
            {
                await _projectPlanAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


        public IActionResult OpenProjectPlanForm(int? id, int projectId)
        {
            return ViewComponent("ProjectPlanForm", new { reportId = id, projectId });
        }

        #endregion

        #region ProjectMedia

        [HttpPost]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> AddProjectMedia(ProjectViewModel model, ICollection<IFormFile> files)
        {
            try
            {
                if (model.ProjectMediaForm != null)
                {
                    if (files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            var uniqueFileName = GetUniqueFileName(file.FileName);
                            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                            var filePath = Path.Combine(uploads, uniqueFileName);
                            await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
                            var url = Url.Content("~/uploads/" + uniqueFileName);

                            model.ProjectMediaForm.ImageUrl = url;

                            await _projectMediaAppService.Create(new ProjectMediaDto()
                            {
                                Title = model.ProjectMediaForm.Title,
                                ImageUrl = model.ProjectMediaForm.ImageUrl,
                                SubmitDate = PersianDateTime.Parse(model.ProjectMediaForm.SubmitDate),
                                ProjectId = model.ProjectMediaForm.ProjectId,
                            });
                        }


                    }

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
        public async Task<ActionResult> ProjectMediaRead([DataSourceRequest] DataSourceRequest request, int projectId)
        {
            var items = await _projectMediaAppService.GetProjectMedias(projectId);
            var reports = items.Select(a => new ProjectMediaViewModel()
            {
                Id = a.Id,
                Title = a.Title,
                ProjectId = a.ProjectId,
                SubmitDate = new PersianDateTime(a.SubmitDate).ToShortDateString(),
                ImageUrl = a.ImageUrl
            }).ToList();
            var dsResult = reports.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        [AcceptVerbs("Post")]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> ProjectMediaDestroy([DataSourceRequest] DataSourceRequest request, ProjectMedia item)
        {
            if (item != null)
            {
                await _projectMediaAppService.Delete(new EntityDto(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }
        #endregion


        [HttpPost]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> AddProjectAsset(int projectId, IFormFile file, string LastAssetFile)
        {
            try
            {
                if (file != null && !string.IsNullOrEmpty(LastAssetFile))
                {
                    var uniqueFileName = GetUniqueFileName(file.FileName);
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
                    var url = Url.Content("~/uploads/" + uniqueFileName);

                    var project = await _projectAppService.Get(new EntityDto<int>(projectId));
                    project.AssetFileUrl = uniqueFileName;
                    project.AssetFileLastModifyDate = PersianDateTime.Parse(LastAssetFile);

                    await _projectAppService.Update(project);

                    return Json(new { status = "success" });

                }
                return Json(new { status = "error" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost]
        [DontWrapResult]
        [DisableValidation]
        public async Task<IActionResult> DeleteAssetFile(int projectId)
        {
            try
            {
                var project = await _projectAppService.Get(new EntityDto<int>(projectId));
                project.AssetFileUrl = null;
                await _projectAppService.Update(project);

                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }





        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 6)
                      + Path.GetExtension(fileName);
        }
    }
}

#endregion

