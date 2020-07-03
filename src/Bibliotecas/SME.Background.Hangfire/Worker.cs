using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.Background.Core.Interfaces;
using SME.Background.Hangfire.Logging;
using SME.SGP.Aplicacao.Pipelines;
using System;
using System.IO;

namespace SME.Background.Hangfire
{
    public class Worker : IWorker
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        private readonly IServiceCollection serviceCollection;
        private BackgroundJobServer hangFireServer;
        private IWebHost host;

        public Worker(IConfiguration configuration, IServiceCollection serviceCollection, string connectionString)
        {
            this.configuration = configuration;
            this.serviceCollection = serviceCollection;
            this.connectionString = (!connectionString.EndsWith(';') ? connectionString + ";" : connectionString) + "Application Name=SGP Worker Service";
        }

        public void Dispose()
        {
            host?.Dispose();
            hangFireServer.Dispose();
        }

        public void Registrar()
        {
            RegistrarHangfireServer();
            RegistrarDashboard();
        }

        private void RegistrarDashboard()
        {
            host = new WebHostBuilder()
                           .UseKestrel()
                           .UseContentRoot(Directory.GetCurrentDirectory())
                           .ConfigureAppConfiguration((hostContext, config) =>
                           {
                               config.SetBasePath(Directory.GetCurrentDirectory());
                               config.AddEnvironmentVariables();
                           })
                           .UseStartup<Startup>()
                           .UseUrls(new[] { "http://*:5000" })
                           .Build();

            host.RunAsync();
        }

        private void RegistrarHangfireServer()
        {
            var pollInterval = configuration.GetValue<int>("BackgroundWorkerQueuePollInterval", 5);
            Console.WriteLine($"SGP Worker Service - BackgroundWorkerQueuePollInterval parameter = {pollInterval}");


            //TODO VERIFICAR AddTransient
            var assembly = AppDomain.CurrentDomain.Load("SME.SGP.Aplicacao");
            serviceCollection.AddMediatR(assembly);
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidacoesPipeline<,>));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mediator = (IMediator)serviceProvider.GetService(typeof(IMediator));


            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseLogProvider<SentryLogProvider>(new SentryLogProvider())
                .UseRecommendedSerializerSettings()
                .UseActivator(new HangfireActivator(serviceCollection.BuildServiceProvider(), mediator))
                .UseFilter<AutomaticRetryAttribute>(new AutomaticRetryAttribute() { Attempts = 0 })
                .UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromSeconds(pollInterval),
                    SchemaName = "hangfire"
                });

            GlobalJobFilters.Filters.Add(new SGP.Hangfire.ContextFilterAttribute());

            var workerCount = configuration.GetValue<int>("BackgroundWorkerParallelDegree", 1);
            Console.WriteLine($"SGP Worker Service - BackgroundWorkerParallelDegree parameter = {workerCount}");

            hangFireServer = new BackgroundJobServer(new BackgroundJobServerOptions()
            {
                WorkerCount = workerCount
            });
        }
    }
}

//