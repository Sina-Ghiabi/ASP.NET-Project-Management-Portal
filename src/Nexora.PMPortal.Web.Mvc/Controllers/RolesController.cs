using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Nexora.PMPortal.Authorization;
using Nexora.PMPortal.Controllers;
using Nexora.PMPortal.Roles;
using Nexora.PMPortal.Roles.Dto;
using Nexora.PMPortal.Web.Models.Roles;
using Abp.Web.Models;
using Kendo.Mvc.UI;
using Nexora.PMPortal.Users.Dto;
using System.Linq;
using MD.PersianDateTime;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Abp.Runtime.Validation;

namespace Nexora.PMPortal.Web.Controllers
{
    [AbpMvcAuthorize]
    public class RolesController : PMPortalControllerBase
    {
        private readonly IRoleAppService _roleAppService;

        public RolesController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = (await _roleAppService.GetRolesAsync(new GetRolesInput())).Items;
            var permissions = (await _roleAppService.GetAllPermissions()).Items;
            var model = new RoleListViewModel
            {
                Roles = roles,
                Permissions = permissions
            };

            return View(model);
        }

        [DontWrapResult]
        public async Task<ActionResult> RoleRead([DataSourceRequest]DataSourceRequest request)
        {
            var roles = await _roleAppService.GetRolesAsync(new GetRolesInput());
            var result = roles.Items.Select(i => new RoleListDto
            {
                Id = i.Id,
                Name = i.Name,
                DisplayName = i.DisplayName,
                IsDefault = i.IsDefault,
                IsStatic = i.IsStatic,
                CreationTime = i.CreationTime
            }).ToList();

            var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [AcceptVerbs("Post")]
        [DontWrapResult]
        public async Task<IActionResult> RoleDestroy([DataSourceRequest] DataSourceRequest request, RoleListDto item)
        {
            if (item != null)
            {
                await _roleAppService.Delete(new EntityDto<int>(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }


        public async Task<ActionResult> Edit(int id)
        {
            var output = await _roleAppService.GetRoleForEdit(new EntityDto(id));
            var model = ObjectMapper.Map<EditRoleModalViewModel>(output);

            return View(model);
        }

        [HttpPost]
        [DisableValidation]
        public async Task<ActionResult> Edit(EditRoleModalViewModel model)
        {
            var item = new RoleDto()
            {
                Id = model.Role.Id,
                Description = model.Role.Description,
                DisplayName = model.Role.DisplayName,
                GrantedPermissions = model.GrantedPermissionNames,
                Name = model.Role.Name
            };
            var result = await _roleAppService.Update(item);
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Create()
        {
            var output = (await _roleAppService.GetAllPermissions()).Items;
            var model = new EditRoleModalViewModel()
            {
                 Permissions = output.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [DisableValidation]
        public async Task<ActionResult> Create(EditRoleModalViewModel model)
        {
            var item = new CreateRoleDto()
            {
                Description = model.Role.Description,
                DisplayName = model.Role.DisplayName,
                GrantedPermissions = model.GrantedPermissionNames,
                Name = model.Role.Name,
            };
            var result = await _roleAppService.Create(item);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditRoleModal(int roleId)
        {
            var output = await _roleAppService.GetRoleForEdit(new EntityDto(roleId));
            var model = ObjectMapper.Map<EditRoleModalViewModel>(output);

            return View("_EditRoleModal", model);
        }
    }
}
