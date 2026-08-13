using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using Nexora.PMPortal.Authorization.Roles;
using Nexora.PMPortal.Authorization.Users;
using Nexora.PMPortal.Configuration;
using Nexora.PMPortal.Localization;
using Nexora.PMPortal.MultiTenancy;
using Nexora.PMPortal.Timing;

namespace Nexora.PMPortal
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class PMPortalCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            PMPortalLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = PMPortalConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMPortalCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
