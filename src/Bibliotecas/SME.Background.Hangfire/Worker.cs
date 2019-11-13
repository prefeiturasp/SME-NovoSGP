using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SME.Background.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SME.Background.Hangfire
{
    public class Worker : IWorker
    {
        readonly IConfiguration configuration;
        BackgroundJobServer hangFireServer;
        IDisposable dashboard;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
            hangFireServer?.Dispose();
            dashboard?.Dispose();
        }

        public void Registrar()
        {
            //GlobalConfiguration.Configuration
            //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //    .UseSimpleAssemblyNameTypeSerializer()
            //    .UseRecommendedSerializerSettings()
            //    .UseFilter<AutomaticRetryAttribute>(new AutomaticRetryAttribute() { Attempts = 0 })
            //    .UsePostgreSqlStorage(configuration.GetConnectionString("SGP-Postgres"), new PostgreSqlStorageOptions()
            //    {
            //        QueuePollInterval = TimeSpan.FromSeconds(1),
            //        InvisibilityTimeout = TimeSpan.FromMinutes(1),
            //        SchemaName = "hangfire"
            //    });

            //hangFireServer = new BackgroundJobServer();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHangfire(configuration => configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseFilter<AutomaticRetryAttribute>(new AutomaticRetryAttribute() { Attempts = 0 })
                        .UsePostgreSqlStorage(hostContext.Configuration.GetConnectionString("SGP-Postgres"), new PostgreSqlStorageOptions()
                        {
                            QueuePollInterval = TimeSpan.FromSeconds(1),
                            InvisibilityTimeout = TimeSpan.FromMinutes(1),
                            SchemaName = "hangfire"
                        }));
                    services.AddHangfireServer();
                })
                .UseStartup<Startup>()
                .Build();

            host.RunAsync();
        }
    }
}
//