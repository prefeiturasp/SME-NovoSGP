using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SME.SGP.Infra
{
    public class HealthChecksBuilderSgp
    {
        private readonly IHealthChecksBuilder healthChecksBuilder;

        public HealthChecksBuilderSgp(IServiceCollection services)
        {
            healthChecksBuilder = services.AddHealthChecks();
        }

        public HealthChecksBuilderSgp AddNpgSqlSgp(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SGP_Postgres");
            healthChecksBuilder.AddNpgSql(connectionString, name: "Postgres");
            return this;
        }

        public HealthChecksBuilderSgp AddRedisSgp()
        {
            healthChecksBuilder.AddCheck<RedisCheck>("Redis");
            return this;
        }

        public HealthChecksBuilderSgp AddRabbitMqSgp(IConfiguration configuration)
        {
            const string configurationSection = "ConfiguracaoRabbit";

            var userName = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Username").Value);
            var password = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Password").Value);
            var hostName = configuration.GetSection($"{configurationSection}:Hostname").Value;
            var vHost = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Virtualhost").Value);

            var connectionString = $"amqp://{userName}:{password}@{hostName}/{vHost}";
            healthChecksBuilder.AddRabbitMQ(connectionString, name: "RabbitMQ", failureStatus: HealthStatus.Unhealthy);

            return this;
        }

        public HealthChecksBuilderSgp AddElasticSearchSgp()
        {
            healthChecksBuilder.AddCheck<ElasticSearchCheck>("Elastic Search");
            return this;
        }

        public IHealthChecksBuilder Builder()
        {
            return healthChecksBuilder;
        }
    }
}