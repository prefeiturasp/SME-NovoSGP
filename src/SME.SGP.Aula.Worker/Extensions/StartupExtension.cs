using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aula.Worker.Extensions
{
    public static class StartupExtension
    {
        public static void ConfigurarVariaveisAmbiente(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddOptions<ConfiguracaoRabbitOptions>().Bind(configuration.GetSection(nameof(ConfiguracaoRabbitOptions)));
        }

        public static void ConfigurarTelemetria(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<TelemetriaOptions>().Bind(configuration.GetSection(TelemetriaOptions.Secao)).Configure(options =>
            {
                
            });
        }
    }
}
