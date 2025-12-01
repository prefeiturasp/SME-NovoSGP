using System;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Nest;

namespace SME.SGP.Infra
{
    public class ElasticSearchCheck : IHealthCheck
    {
        private readonly IElasticClient elasticClient;

        public ElasticSearchCheck(IElasticClient elasticClient)
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
            var response = await elasticClient.Cluster.HealthAsync(new ClusterHealthRequest { WaitForStatus = WaitForStatus.Red }, cancellationToken);
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