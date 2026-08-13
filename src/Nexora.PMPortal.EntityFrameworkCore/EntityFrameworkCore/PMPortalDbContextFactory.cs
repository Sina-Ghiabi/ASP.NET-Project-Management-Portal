using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Nexora.PMPortal.Configuration;
using Nexora.PMPortal.Web;

namespace Nexora.PMPortal.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class PMPortalDbContextFactory : IDesignTimeDbContextFactory<PMPortalDbContext>
    {
        public PMPortalDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PMPortalDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            PMPortalDbContextConfigurer.Configure(builder, configuration.GetConnectionString(PMPortalConsts.ConnectionStringName));

            return new PMPortalDbContext(builder.Options);
        }
    }
}
