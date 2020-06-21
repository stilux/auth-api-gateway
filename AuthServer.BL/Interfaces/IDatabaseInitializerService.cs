using System;

namespace AuthServer.BL.Interfaces
{
    public interface IDatabaseInitializerService
    {
        void Initialize(IServiceProvider serviceProvider);
    }
}