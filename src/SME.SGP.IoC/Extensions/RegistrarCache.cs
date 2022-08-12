using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.IoC
{
    internal static class RegistrarCache
    {
        internal static void AdicionarRedis(this IServiceCollection services, IConfiguration configuration, IServicoLog servicoLog)
        {
            services.AddSingleton<IConnectionMultiplexerSME>(
                new ConnectionMultiplexerSME(configuration.GetConnectionString("SGP_Redis"), servicoLog));
        }
    }
}
