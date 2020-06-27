using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthServer.DAL.Context
{
    public class IdentityDbContextFactory : DesignTimeDbContextFactory<IdentityDbContext>
    {
        protected override IdentityDbContext CreateDbContext(DbContextOptions<IdentityDbContext> options)
        {
            return new IdentityDbContext(options);
        }
    }
}