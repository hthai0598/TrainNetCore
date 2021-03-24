using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseNetCore3._1.Installer
{
    public class InstallerSystem : IInstaller
    {
        public void InstallerService(IServiceCollection service, IConfiguration configuration)
        {
            service.AddControllers();
        }
    }
}
