using Abp.AspNetCore.Mvc.ViewComponents;

namespace Nexora.PMPortal.Web.Views
{
    public abstract class PMPortalViewComponent : AbpViewComponent
    {
        protected PMPortalViewComponent()
        {
            LocalizationSourceName = PMPortalConsts.LocalizationSourceName;
        }
    }
}
