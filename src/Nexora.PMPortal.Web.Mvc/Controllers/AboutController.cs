using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Nexora.PMPortal.Controllers;

namespace Nexora.PMPortal.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : PMPortalControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
