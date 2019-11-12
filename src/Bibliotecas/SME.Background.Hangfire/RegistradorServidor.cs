using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using SME.Background.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Hangfire
{
    public class RegistradorServidor : IRegistrador, IDisposable
    {
        readonly IConfiguration configuration;
        BackgroundJobServer hangFireServer;

        public RegistradorServidor(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
            hangFireServer.Dispose();
        }

        public void Registrar()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(configuration.GetConnectionString("SGP-Postgres"), new PostgreSqlStorageOptions()
                {
                    QueuePollInterval = TimeSpan.Zero,
                    InvisibilityTimeout = TimeSpan.FromMinutes(1)
                });

            hangFireServer = new BackgroundJobServer();
        }
    }
}
