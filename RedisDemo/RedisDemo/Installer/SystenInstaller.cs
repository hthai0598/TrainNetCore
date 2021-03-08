using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RedisDemo.Installer
{
    public class SystenInstaller : IInstaller
    {
        public SystenInstaller()
        {
        }

        public void InstallerService(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
