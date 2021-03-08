using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RedisDemo.Installer
{
    public static class InstallerExtensions
    {
        public static void InstallerServiceInAssembly(this IServiceCollection service, IConfiguration configuration)
        {
            //lay ra het cac class trong thu muc install, bo di nhung interface, abstract
            var installer = typeof(Startup).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller)
                .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installer.ForEach(installer => installer.InstallerService(service, configuration));
        }
    }
}
