using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RedisDemo.Installer
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallerService(IServiceCollection service, IConfiguration configuration)
        {
            service.AddControllers();
        }
    }
}
