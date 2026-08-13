using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Nexora.PMPortal.Configuration;

namespace Nexora.PMPortal.Web.Startup
{
    [DependsOn(typeof(PMPortalWebCoreModule))]
    public class PMPortalWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public PMPortalWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<PMPortalNavigationProvider>();
            Configuration.MultiTenancy.IsEnabled = false;
            Configuration.Auditing.IsEnabled = false;

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMPortalWebMvcModule).GetAssembly());
        }
    }
}
