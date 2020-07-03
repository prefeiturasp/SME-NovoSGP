using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SME.SGP.IoC
{
    public static class RegistrarCache
    {
        public static void AdicionarRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("SGP-Redis");
                options.InstanceName = configuration.GetValue<string>("Nome-Instancia-Redis");
            });
        }
    }
}
