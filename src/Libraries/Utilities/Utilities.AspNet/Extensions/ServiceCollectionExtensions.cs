using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using Utilities.Extensions;
using Utilities.Settings;

namespace Utilities.AspNet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) { throw new ArgumentNullException(nameof(services)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            services.AddConfig<Redis>(configuration);

            var redisOptions = new Redis();
            configuration.GetSection("Redis").Bind(redisOptions);

            var connectionString = redisOptions.ConnectionString;

            var redis = ConnectionMultiplexer.Connect(connectionString);

            services.AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        }
    }
}