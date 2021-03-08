using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RedisDemo.Installer
{
    public interface IInstaller
    {
        void InstallerService(IServiceCollection service, IConfiguration configuration);
    }
}
