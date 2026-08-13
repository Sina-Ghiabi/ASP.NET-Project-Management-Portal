using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Nexora.PMPortal.EntityFrameworkCore
{
    public static class PMPortalDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<PMPortalDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<PMPortalDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
