using System;
using System.Threading;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Cluster;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SME.SGP.Infra
{
    public class ElasticSearchCheck : IHealthCheck
    {
        private readonly ElasticsearchClient elasticClient;

        public ElasticSearchCheck(ElasticsearchClient elasticClient)
        {
            this.elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                return await VerificarStatusElasticSearch(cancellationToken);
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy($"O serviço Elastic Search apresenta problemas: {e.Message}");
            }
        }

        private async Task<HealthCheckResult> VerificarStatusElasticSearch(CancellationToken cancellationToken)
        {
            var response = await elasticClient.Cluster.HealthAsync(new HealthRequest(), cancellationToken);
            var healthColor = response.Status.ToString();

            return healthColor.ToLower() switch
            {
                "green" => HealthCheckResult.Healthy($"O serviço ElasticSearch está respondendo normalmente - {healthColor.ToUpper()}."),
                "yellow" => HealthCheckResult.Healthy($"O serviço ElasticSearch está respondendo normalmente - {healthColor.ToUpper()} (é normal para clusters de nó único)."),
                _ => HealthCheckResult.Unhealthy($"O serviço ElasticSearch apresenta problemas - {healthColor.ToUpper()}")
            };            
        }
    }
}