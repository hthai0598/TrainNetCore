using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisDemo.Configurations;
using RedisDemo.Service;
using StackExchange.Redis;

namespace RedisDemo.Installer
{
    public class CacheInstaller : IInstaller
    {

        public void InstallerService(IServiceCollection service, IConfiguration configuration)
        {
            var redisConfigurations = new RedisConfigurations();
            configuration.GetSection("RedisConfigurations").Bind(redisConfigurations);

            service.AddSingleton(redisConfigurations);

            if (!redisConfigurations.Enable)
                return;
            service.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(redisConfigurations.ConnectionStrings));
            service.AddStackExchangeRedisCache(option => option.Configuration = redisConfigurations.ConnectionStrings);
            service.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
