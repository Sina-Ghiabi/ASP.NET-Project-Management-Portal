using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Nexora.PMPortal.Configuration.Dto;

namespace Nexora.PMPortal.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : PMPortalAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
