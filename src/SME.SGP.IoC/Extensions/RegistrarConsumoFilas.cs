using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.IoC
{
    internal static class RegistrarConsumoFilas
    {
        internal static void ConfigurarConsumoFilas(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ConsumoFilasOptions>()
                .Bind(configuration.GetSection("ConsumoFilas"), c => c.BindNonPublicProperties = true);

            services.AddSingleton<ConsumoFilasOptions>();
        }
    }
}
