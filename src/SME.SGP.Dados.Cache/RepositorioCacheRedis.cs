using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Dados.Cache;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCacheRedis : RepositorioCache
    {
        private readonly IDatabase redis;
        private readonly RedisOptions redisOptions;

        public RepositorioCacheRedis(IConnectionMultiplexerSME connectionMultiplexerSme,
                                     IServicoTelemetria servicoTelemetria,
                                     RedisOptions redisOptions,
                                     IServicoMensageriaLogs servicoMensageriaLogs,
                                     IMetricasCache metricasCache) 
            : base(servicoTelemetria, servicoMensageriaLogs, metricasCache)
        {
            ConnectionMultiplexerSme = connectionMultiplexerSme ?? throw new ArgumentNullException(nameof(connectionMultiplexerSme));
            redis = connectionMultiplexerSme.GetDatabase() ?? throw new ArgumentNullException(nameof(connectionMultiplexerSme.GetDatabase), "RedisDatabase");
            this.redisOptions = redisOptions ?? throw new ArgumentNullException(nameof(redisOptions));
            NomeServicoCache = "Cache Redis";
        }
        
        public IConnectionMultiplexerSME ConnectionMultiplexerSme { get; }

        protected override string ObterValor(string nomeChave)
        {
            try
            {
                return redis.StringGet(string.Concat(redisOptions.Prefixo, nomeChave));
            }
            catch (RedisTimeoutException)
            {
                return default;
            }
            catch (RedisConnectionException)
            {
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override Task RemoverValor(string nomeChave)
        {
            try
            {
                return redis.KeyDeleteAsync(string.Concat(redisOptions.Prefixo, nomeChave));
            }
            catch (RedisTimeoutException)
            {
                return Task.CompletedTask;
            }
            catch (RedisConnectionException)
            {
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar)
        {
            try
            {
               return redis.StringSetAsync(new RedisKey(string.Concat(redisOptions.Prefixo, nomeChave)), new RedisValue(valor), TimeSpan.FromMinutes(minutosParaExpirar));
            }
            catch (RedisTimeoutException)
            {
                return Task.CompletedTask;
            }
            catch (RedisConnectionException)
            {
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
