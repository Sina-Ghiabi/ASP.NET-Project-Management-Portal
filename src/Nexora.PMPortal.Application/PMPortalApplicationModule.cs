using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Nexora.PMPortal.Authorization;

namespace Nexora.PMPortal
{
    [DependsOn(
        typeof(PMPortalCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class PMPortalApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<PMPortalAuthorizationProvider>();
            Configuration.MultiTenancy.IsEnabled = false;
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(PMPortalApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
