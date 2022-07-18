using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SME.SGP.Dados;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.IoC
{
    internal static class RegistrarTelemetria
    {
        internal static void ConfigurarTelemetria(this IServiceCollection services, IConfiguration configuration)
        {
            // if (configuration == null)
                return;

            services.AddApplicationInsightsTelemetry(configuration);

            services.AddOptions<TelemetriaOptions>()
                .Bind(configuration.GetSection(TelemetriaOptions.Secao), c => c.BindNonPublicProperties = true);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<TelemetriaOptions>>();
            //var clientTelemetry = serviceProvider.GetService<TelemetryClient>();
            //var servicoTelemetria = new ServicoTelemetria(clientTelemetry, options);
            //DapperExtensionMethods.Init(servicoTelemetria);

            //services.AddSingleton(servicoTelemetria);
            services.AddSingleton<TelemetriaOptions>();
            services.AddSingleton<IServicoTelemetria, ServicoTelemetria>();
        }
    }
}
