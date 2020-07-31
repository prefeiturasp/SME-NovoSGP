using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using SME.SGP.Dados;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Worker.Service
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // Run with console or service
            var asService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder()
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddEnvironmentVariables();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration(context.Configuration);
                logging.AddSentry();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<WorkerService>();
                WorkerService.ConfigurarDependencias(hostContext.Configuration, services);
                WorkerService.Configurar(hostContext.Configuration, services);

                services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                    .Connect(string.Concat(hostContext.Configuration.GetConnectionString("SGP-Redis"), $",ConnectTimeout={TimeSpan.FromSeconds(1).TotalMilliseconds}")));

                services.AddApplicationInsightsTelemetryWorkerService(hostContext.Configuration.GetValue<string>("ApplicationInsights__InstrumentationKey"));

                // Teste para injeção do client de telemetria em classe estática                 

                var telemetryConfiguration = new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration(hostContext.Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));

                var telemetryClient = new TelemetryClient(telemetryConfiguration);

                DapperExtensionMethods.Init(telemetryClient);

                //

            });

            builder.UseEnvironment(asService ? EnvironmentName.Production : EnvironmentName.Development);

            if (asService)
            {
                await builder.Build().RunAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }
    }
}