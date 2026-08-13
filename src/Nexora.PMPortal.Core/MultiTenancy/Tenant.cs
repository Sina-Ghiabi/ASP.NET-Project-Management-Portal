using Abp.MultiTenancy;
using Nexora.PMPortal.Authorization.Users;

namespace Nexora.PMPortal.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
