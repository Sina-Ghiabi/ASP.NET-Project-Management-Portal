using Abp.AutoMapper;
using Nexora.PMPortal.Authentication.External;

namespace Nexora.PMPortal.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
