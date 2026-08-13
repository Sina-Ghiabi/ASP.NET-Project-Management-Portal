using Microsoft.AspNetCore.Antiforgery;
using Nexora.PMPortal.Controllers;

namespace Nexora.PMPortal.Web.Host.Controllers
{
    public class AntiForgeryController : PMPortalControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
