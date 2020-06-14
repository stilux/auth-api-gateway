using System;
using System.Linq;
using System.Security.Claims;
using AuthServer.DAL.Context;
using AuthServer.DAL.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthServer.DAL
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            
            serviceProvider.GetRequiredService<IdentityDbContext>().Database.Migrate();
            
            InitializeConfigurationDb(serviceProvider);
        }
        
        private static void InitializeConfigurationDb(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.Ids)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.Apis)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}