using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace SME.SGP.IoC
{
    public static class RegistrarCache
    {
        public static void AdicionarRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                .Connect(string.Concat(configuration.GetConnectionString($"SGP-Redis"), $",ConnectTimeout={TimeSpan.FromSeconds(1).TotalMilliseconds}")));
        }
    }
}
