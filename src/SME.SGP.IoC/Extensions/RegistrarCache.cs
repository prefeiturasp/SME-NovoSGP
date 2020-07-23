using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.IoC
{
    public static class RegistrarCache
    {
        public static void AdicionarRedis(this IServiceCollection services, IConfiguration configuration, IServicoLog servicoLog)
        {
            services.AddSingleton<IConnectionMultiplexerSME>(
                new ConnectionMultiplexerSME(configuration.GetConnectionString("SGP-Redis"), servicoLog));
        }
    }
}
