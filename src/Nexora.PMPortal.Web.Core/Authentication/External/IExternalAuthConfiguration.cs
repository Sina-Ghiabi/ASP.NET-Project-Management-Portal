using System.Collections.Generic;

namespace Nexora.PMPortal.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
