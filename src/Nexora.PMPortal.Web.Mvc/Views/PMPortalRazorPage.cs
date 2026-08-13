using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace Nexora.PMPortal.Web.Views
{
    public abstract class PMPortalRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected PMPortalRazorPage()
        {
            LocalizationSourceName = PMPortalConsts.LocalizationSourceName;
        }
    }
}
