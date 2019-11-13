using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using SME.Background.Core.Enumerados;
using SME.Background.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace SME.Background.Hangfire
{
    public class ProcessadorCliente : IProcessadorCliente
    {
        readonly IConfiguration configuration;

        public ProcessadorCliente(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Executar(Expression<Action> metodo)
        {
            return BackgroundJob.Enqueue(metodo);
        }

        public void ExecutarPeriodicamente(Expression<Action> metodo, string cron)
        {
            RecurringJob.AddOrUpdate(metodo, cron);
        }

        public EstadoProcessamento ObterEstadoProcessamento(string idCorrelato)
        {
            throw new NotImplementedException();
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
