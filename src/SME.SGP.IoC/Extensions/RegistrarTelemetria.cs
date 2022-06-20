using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.IoC
{
    internal static class RegistrarTelemetria
    {
        internal static void ConfigurarTelemetria(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<TelemetriaOptions>()
                .Bind(configuration.GetSection(TelemetriaOptions.Secao), c => c.BindNonPublicProperties = true)
                .Configure(options =>
            {
                services.AddApplicationInsightsTelemetry(configuration);

                var serviceProvider = services.BuildServiceProvider();
                var clientTelemetry = serviceProvider.GetService<TelemetryClient>();
                var servicoTelemetria = new ServicoTelemetria(clientTelemetry, options);

                DapperExtensionMethods.Init(servicoTelemetria);

                services.AddSingleton(servicoTelemetria);
            });

            services.AddSingleton<TelemetriaOptions>();
        }
    }
}
