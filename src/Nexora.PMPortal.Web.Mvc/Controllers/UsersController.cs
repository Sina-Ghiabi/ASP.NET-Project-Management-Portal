using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Nexora.PMPortal.Authorization;
using Nexora.PMPortal.Controllers;
using Nexora.PMPortal.Users;
using Nexora.PMPortal.Web.Models.Users;
using Nexora.PMPortal.Users.Dto;
using Nexora.PMPortal.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Abp.Web.Models;
using Kendo.Mvc.UI;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MD.PersianDateTime;
using Kendo.Mvc.Extensions;
using Abp.Runtime.Validation;

namespace Nexora.PMPortal.Web.Controllers
{
    [AbpMvcAuthorize]
    public class UsersController : PMPortalControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(IUserAppService userAppService, UserManager userManager, IPasswordHasher<User> passwordHasher)
        {
            _userAppService = userAppService;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<ActionResult> Index()
        {
            var users = (await _userAppService.GetAll(new PagedUserResultRequestDto { MaxResultCount = int.MaxValue })).Items; // Paging not implemented yet
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new UserListViewModel
            {
                Users = users,
                Roles = roles
            };
            return View(model);
        }

        [DontWrapResult]
        public async Task<ActionResult> UserRead([DataSourceRequest]DataSourceRequest request)
        {
            var users = await _userAppService.GetAll(new PagedUserResultRequestDto { MaxResultCount = int.MaxValue });
            var result = users.Items.Select(i => new UserViewModel
            {
                Id = i.Id,
                Name = i.Name,
                EmailAddress = i.EmailAddress,
                FullName = i.FullName,
                Surname = i.Surname,
                UserName = i.UserName,
                IsActive = i.IsActive,
                CreationTime = new PersianDateTime(i.CreationTime).ToShortDateString(),
                LastLoginTime = i.LastLoginTime != null ? new PersianDateTime(i.LastLoginTime).ToShortDateString() : default(string),
                RoleNames = string.Join(" - ", i.RoleNames)
            }).ToList();

            var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult, new JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }


        [AcceptVerbs("Post")]
        [DontWrapResult]
        public async Task<IActionResult> UserDestroy([DataSourceRequest] DataSourceRequest request, UserViewModel item)
        {
            if (item != null)
            {
                await _userAppService.Delete(new EntityDto<long>(item.Id));
            }
            return Json(new[] { item }.ToDataSourceResult(request, ModelState));
        }



        public async Task<ActionResult> Create()
        {
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new UserFormViewModel
            {
                Roles = roles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserFormViewModel model)
        {
            model.User.RoleNames = model.SelectedRoles.Where(a => a != "false").ToArray();
            await _userAppService.Create(model.User);
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Edit(long id)
        {
            var user = await _userManager.GetUserByIdAsync(id);
            var currentUserRoles = await _userManager.GetRolesAsync(user);
            var roles = (await _userAppService.GetRoles()).Items;
            foreach (var item in roles)
            {
                if (currentUserRoles.Contains(item.Name))
                {
                    item.Selected = true;
                }
            }
            var model = new UserFormViewModel
            {
                Roles = roles,
                Id = id,
                User = new CreateUserDto()
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    EmailAddress = user.EmailAddress,
                    IsActive = user.IsActive
                }
            };


            return View(model);
        }

        [HttpPost]
        [DisableValidation]
        public async Task<ActionResult> Edit(UserFormViewModel model)
        {
            var user = await _userManager.GetUserByIdAsync(model.Id);

            var dto = new UserDto()
            {
                Id = model.Id,
                UserName = model.User.UserName,
                Name = model.User.Name,
                Surname = model.User.Surname,
                EmailAddress = model.User.EmailAddress,
                IsActive = model.User.IsActive,
                RoleNames = model.SelectedRoles.Where(a => a != "false").ToArray()
            };

            await _userAppService.Update(dto);
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> ResetPassword(long id)
        {
            var user = await _userManager.GetUserByIdAsync(id);

            var model = new UserFormViewModel
            {
                Id = id,
                User = new CreateUserDto()
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    EmailAddress = user.EmailAddress,
                    IsActive = user.IsActive
                }
            };


            return View(model);
        }

        [HttpPost]
        [DisableValidation]
        public async Task<ActionResult> ResetPassword(UserFormViewModel model)
        {
            var user = await _userManager.GetUserByIdAsync(model.Id);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, model.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> EditUserModal(long userId)
        {
            var user = await _userAppService.Get(new EntityDto<long>(userId));
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new EditUserModalViewModel
            {
                User = user,
                Roles = roles
            };
            return View("_EditUserModal", model);
        }

        public IActionResult ProjectListModal(long userId)
        {
            return ViewComponent("UserProjectModal", new { userId });
        }
    }
}
