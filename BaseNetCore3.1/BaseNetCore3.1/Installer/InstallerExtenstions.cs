using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseNetCore3._1.Installer
{
    public static class InstallerExtenstions
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
