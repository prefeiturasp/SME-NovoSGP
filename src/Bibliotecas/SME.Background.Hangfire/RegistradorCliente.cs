using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using SME.Background.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Hangfire
{
    public class RegistradorCliente : IRegistrador
    {
        readonly IConfiguration configuration;

        public RegistradorCliente(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Registrar()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(configuration.GetConnectionString("SGP-Postgres"));
        }
    }
}
