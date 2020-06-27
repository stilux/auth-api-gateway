using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.DAL.Context
{
    public class PersistedGrantDbContextFactory : DesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        protected override PersistedGrantDbContext CreateDbContext(DbContextOptions<PersistedGrantDbContext> options)
        {
            return new PersistedGrantDbContext(options, new OperationalStoreOptions());
        }
    }
}