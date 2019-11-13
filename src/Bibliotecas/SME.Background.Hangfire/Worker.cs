using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using SME.Background.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Hangfire
{
    public class Worker : IWorker
    {
        readonly IConfiguration configuration;
        BackgroundJobServer hangFireServer;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
            hangFireServer?.Dispose();
        }

        public void Registrar()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(configuration.GetConnectionString("SGP-Postgres"), new PostgreSqlStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromSeconds(1),
                    InvisibilityTimeout = TimeSpan.FromMinutes(1)
                });

            hangFireServer = new BackgroundJobServer();
        }
    }
}
