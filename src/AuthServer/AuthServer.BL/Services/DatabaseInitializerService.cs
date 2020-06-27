using System;
using AuthServer.BL.Interfaces;
using AuthServer.DAL;

namespace AuthServer.BL.Services
{
    public class DatabaseInitializerService : IDatabaseInitializerService
    {
        public void Initialize(IServiceProvider serviceProvider)
        {
            DatabaseInitializer.Initialize(serviceProvider);
        }
    }
}