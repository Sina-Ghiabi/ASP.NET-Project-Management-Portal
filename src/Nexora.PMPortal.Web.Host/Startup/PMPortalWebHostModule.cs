using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Nexora.PMPortal.Configuration;

namespace Nexora.PMPortal.Web.Host.Startup
{
    [DependsOn(
       typeof(PMPortalWebCoreModule))]
    public class PMPortalWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public PMPortalWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMPortalWebHostModule).GetAssembly());
        }
    }
}
