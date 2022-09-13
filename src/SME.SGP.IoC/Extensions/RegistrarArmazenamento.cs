using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.IoC
{
    internal static class RegistrarArmazenamento
    {
        internal static void ConfigurarArmazenamento(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConfiguracaoArmazenamentoOptions>()
                .Bind(configuration.GetSection(ConfiguracaoArmazenamentoOptions.Secao), c => c.BindNonPublicProperties = true);

            services.AddSingleton<ConfiguracaoArmazenamentoOptions>();
            services.AddSingleton<IServicoArmazenamento, ServicoArmazenamento>();
        }
    }
}
