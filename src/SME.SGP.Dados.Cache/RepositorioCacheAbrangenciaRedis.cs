using SME.SGP.Dados.Cache;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCacheAbrangenciaRedis : RepositorioCacheRedis, IRepositorioCacheAbrangencia
    {
        public RepositorioCacheAbrangenciaRedis(
            IConnectionMultiplexerSME connectionMultiplexerSme,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaLogs servicoMensageriaLogs,
            IMetricasCache metricasCache,
            RedisOptions redisOptions,
            ICircuitBreaker circuitBreaker)
            : base(connectionMultiplexerSme, servicoTelemetria, servicoMensageriaLogs, metricasCache, redisOptions, circuitBreaker)
        {
            NomeServicoCache = "Cache Redis Abrangência";
        }
    }
}
