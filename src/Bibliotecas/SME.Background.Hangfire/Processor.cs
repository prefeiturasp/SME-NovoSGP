using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using SME.Background.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace SME.Background.Hangfire
{
    public class Processor : IProcessor
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public Processor(IConfiguration configuration, string connectionString)
        {
            this.configuration = configuration;
            this.connectionString = connectionString;
        }

        public bool Registrado { get; private set; }

        public string Executar(Expression<Action> metodo)
        {
            return BackgroundJob.Enqueue(metodo);
        }

        public string Executar<T>(Expression<Action<T>> metodo)
        {
            return BackgroundJob.Enqueue<T>(metodo);
        }

        public void ExecutarPeriodicamente(Expression<Action> metodo, string cron)
        {
            RecurringJob.AddOrUpdate(metodo, cron);
        }

        public void ExecutarPeriodicamente<T>(Expression<Action<T>> metodo, string cron, string nomeFila = "default")
        {
            RecurringJob.AddOrUpdate(metodo, cron, queue: nomeFila);
        }

        public void Registrar()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(configuration.GetConnectionString(connectionString), new PostgreSqlStorageOptions()
                {
                    SchemaName = "hangfire"
                });
            GlobalJobFilters.Filters.Add(new SGP.Hangfire.ContextFilterAttribute());

            Registrado = true;
        }
    }
}