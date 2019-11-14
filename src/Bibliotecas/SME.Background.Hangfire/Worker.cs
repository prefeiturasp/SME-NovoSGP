using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.Background.Core.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace SME.Background.Hangfire
{
    public class Worker : IWorker
    {
        readonly IConfiguration configuration;
        readonly IServiceCollection serviceCollection;
        readonly string connectionString;
        IWebHost host;
        BackgroundJobServer hangFireServer;

        public Worker(IConfiguration configuration, IServiceCollection serviceCollection, string connectionString)
        {
            this.configuration = configuration;
            this.serviceCollection = serviceCollection;
            this.connectionString = connectionString;
        }

        public void Dispose()
        {
            host?.Dispose();
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
                           .Build();

            host.RunAsync();
        }

        private void RegistrarHangfireServer()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseActivator<HangfireActivator>(new HangfireActivator(serviceCollection.BuildServiceProvider()))
                .UseFilter<AutomaticRetryAttribute>(new AutomaticRetryAttribute() { Attempts = 0 })
                .UsePostgreSqlStorage(configuration.GetConnectionString(connectionString), new PostgreSqlStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromSeconds(1),
                    InvisibilityTimeout = TimeSpan.FromMinutes(1),
                    SchemaName = "hangfire"
                });

            hangFireServer = new BackgroundJobServer();
        }
    }
}
//