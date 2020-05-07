using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Worker.Service
{
    internal class Program
    {
        private static IHostBuilder CreateBuilder(bool asService)
        {
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

                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = hostContext.Configuration.GetConnectionString("SGP-Redis");
                    options.InstanceName = hostContext.Configuration.GetValue<string>("Nome-Instancia-Redis");
                });

                services.AddApplicationInsightsTelemetryWorkerService(hostContext.Configuration.GetValue<string>("ApplicationInsights__InstrumentationKey"));
            });

            builder.UseEnvironment(asService ? EnvironmentName.Production : EnvironmentName.Development);
            return builder;
        }

        private static async Task Main(string[] args)
        {
            // Run with console or service
            var asService = !(Debugger.IsAttached || args.Contains("--console"));

            IHostBuilder builder = CreateBuilder(asService);

            var sentryDSN = Environment.GetEnvironmentVariable("Sentry__DSN");
            using (SentrySdk.Init(sentryDSN))
            {
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
}