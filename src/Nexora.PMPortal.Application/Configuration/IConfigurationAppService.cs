using System.Threading.Tasks;
using Nexora.PMPortal.Configuration.Dto;

namespace Nexora.PMPortal.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
