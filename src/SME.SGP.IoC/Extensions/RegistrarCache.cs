using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace SME.SGP.IoC
{
    public static class RegistrarCache
    {
        public static void AdicionarRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                .Connect(configuration.GetConnectionString("SGP-Redis")));
        }
    }
}
