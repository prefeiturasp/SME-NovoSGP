using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Api.HealthCheck
{
    public class ApiJuremaCheck : IHealthCheck
    {
        private readonly IConfiguration configuration;

        public ApiJuremaCheck(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var clienteHttp = new HttpClient();
                var result = clienteHttp.GetAsync(configuration.GetSection("UrlApiJurema").Value + "v1/learning_objectives").Result;

                if (result.IsSuccessStatusCode)
                {
                    return Task.FromResult(
                        HealthCheckResult.Healthy("O serviço está respondendo normalmente."));
                }

                return Task.FromResult(
                    HealthCheckResult.Unhealthy("O serviço apresenta problemas."));
            }
            catch (System.Exception)
            {
                return Task.FromResult(
                                 HealthCheckResult.Unhealthy("O serviço apresenta problemas."));
            }
        }
    }
}