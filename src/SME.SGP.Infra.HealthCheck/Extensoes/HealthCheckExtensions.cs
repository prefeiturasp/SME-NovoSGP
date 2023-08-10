using System.Net;
using System.Web;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SME.SGP.Infra
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddPostgreSqlSgp(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SGP_Postgres");
            return builder.AddNpgSql(connectionString, name: "Postgres");            
        }

        public static IHealthChecksBuilder AddRedisSgp(this IHealthChecksBuilder builder)
        {
            return builder.AddCheck<RedisCheck>("Redis");
        }

        public static IHealthChecksBuilder AddRabbitMqSgp(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            const string configurationSection = "ConfiguracaoRabbit";
            return builder.AddRabbitMQ(ObterStringConexaoRabbit(configuration, configurationSection), name: "RabbitMQ",
                failureStatus: HealthStatus.Unhealthy);
        }
        
        public static IHealthChecksBuilder AddRabbitMqLogSgp(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            const string configurationSection = "ConfiguracaoRabbitLog";
            return builder.AddRabbitMQ(ObterStringConexaoRabbit(configuration, configurationSection), name: "RabbitMQLog",
                failureStatus: HealthStatus.Unhealthy);
        }        

        private static string ObterStringConexaoRabbit(IConfiguration configuration, string configurationSection)
        {
            var userName = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Username").Value);
            var password = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Password").Value);
            var hostName = configuration.GetSection($"{configurationSection}:Hostname").Value;
            var vHost = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Virtualhost").Value);
            
            return $"amqp://{userName}:{password}@{hostName}/{vHost}";
        }

        public static IHealthChecksBuilder AddElasticSearchSgp(this IHealthChecksBuilder builder)
        {
            return builder.AddCheck<ElasticSearchCheck>("Elastic Search");
        }

        public static void UseHealthChecksSgp(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }).UseHealthChecksUI();
        }

        public static HealthChecksUIBuilder AddPostgreSqlStorageSgp(this HealthChecksUIBuilder builder, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SGP_Postgres");
            return builder.AddPostgreSqlStorage(connectionString);
        }

        public static HealthChecksUIBuilder AddHealthChecksUiSgp(this IServiceCollection services)
        {
            return services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(5);
                opt.MaximumHistoryEntriesPerEndpoint(10);
                opt.SetApiMaxActiveRequests(1);

                opt.AddHealthCheckEndpoint("Health-API Indicadores", "http://localhost/healthz");
            }).AddInMemoryStorage();
        }
    }
}