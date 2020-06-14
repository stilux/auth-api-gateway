using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthServer.DAL.Context
{
    public abstract class DesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        public TContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Directory.GetCurrentDirectory() + "/../AuthServer/appsettings.Development.json")
                .Build();

            var migrationsAssembly = GetType().Assembly.GetName().Name;
            var connectionString = configuration.GetConnectionString("IdentityServerDBConnection");

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseNpgsql(connectionString,
                builder => { builder.MigrationsAssembly(migrationsAssembly); });

            return CreateDbContext(optionsBuilder.Options);
        }

        protected abstract TContext CreateDbContext(DbContextOptions<TContext> options);
    }
}