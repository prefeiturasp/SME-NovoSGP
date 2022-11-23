using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SME.SGP.Infra
{
    public static class RabbitMQHealthCheckExtension
    {
        public static IHealthChecksBuilder AddRabbitMQ(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            const string configurationSection = "ConfiguracaoRabbit";

            var userName = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Username").Value);
            var password = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Password").Value);
            var hostName = configuration.GetSection($"{configurationSection}:Hostname").Value;
            var vHost = HttpUtility.UrlEncode(configuration.GetSection($"{configurationSection}:Virtualhost").Value);

            var connectionString = $"amqp://{userName}:{password}@{hostName}/{vHost}";
            return builder.AddRabbitMQ(connectionString, name: "RabbitMQ", failureStatus: HealthStatus.Unhealthy);
        }
    }
}