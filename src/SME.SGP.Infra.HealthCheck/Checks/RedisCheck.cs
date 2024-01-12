using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Utilitarios;
using StackExchange.Redis;

namespace SME.SGP.Infra
{
    public class RedisCheck : IHealthCheck
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly RedisOptions redisOptions;
        private readonly ConfiguracaoCacheOptions configuracaoCacheOptions;

        public RedisCheck(IRepositorioCache repositorioCache, IOptions<RedisOptions> redisOptions,
            IOptions<ConfiguracaoCacheOptions> configuracaoCacheOptions)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.redisOptions = redisOptions.Value ?? throw new ArgumentNullException(nameof(redisOptions));
            this.configuracaoCacheOptions = configuracaoCacheOptions.Value ?? throw new ArgumentNullException(nameof(configuracaoCacheOptions));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!configuracaoCacheOptions.UtilizaRedis
                || repositorioCache is not RepositorioCacheRedis repositorioCacheRedis)
                    return HealthCheckResult.Healthy("O serviço Redis não esta ativo. A aplicação está utilizando cache em memória.");

            try
            {
                foreach (var endPoint in repositorioCacheRedis.ConnectionMultiplexerSme.GetEndPoints(configuredOnly: true))
                {
                    if (redisOptions.Proxy == Proxy.Twemproxy)
                    {
                        await repositorioCacheRedis.ConnectionMultiplexerSme.GetDatabase().PingAsync();
                    }
                    else 
                    {
                        var server = repositorioCacheRedis.ConnectionMultiplexerSme.GetServer(endPoint);
                        
                        if (server.ServerType != ServerType.Cluster)
                        {
                            await repositorioCacheRedis.ConnectionMultiplexerSme.GetDatabase().PingAsync();
                            await server.PingAsync();
                        }
                        else
                        {
                            var clusterInfo = await server.ExecuteAsync("CLUSTER", "INFO");

                            if (clusterInfo is not null && !clusterInfo.IsNull)
                            {
                                if (!clusterInfo.ToString()!.Contains("cluster_state:ok"))
                                {
                                    return HealthCheckResult.Unhealthy(
                                        $"O serviço Redis apresenta problemas: CLUSTER não está íntegro para o endpoint {endPoint}");
                                }
                            }
                            else
                            {
                                return HealthCheckResult.Unhealthy(
                                    $"O serviço Redis apresenta problemas: CLUSTER não está íntegro para o endpoint {endPoint}");
                            }
                        }
                    }
                }
 
                return HealthCheckResult.Healthy("O serviço Redis está respondendo normalmente.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"O serviço Redis apresenta problemas: {ex.Message}");
            }
        }
    }
}