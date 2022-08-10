using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCacheRedis : RepositorioCache
    {
        private readonly IConnectionMultiplexerSME connectionMultiplexerSME;
        private readonly IDatabase redis;
        private readonly RedisOptions redisOptions;

        public RepositorioCacheRedis(IConnectionMultiplexerSME connectionMultiplexerSME,
            IServicoTelemetria servicoTelemetria,
            RedisOptions redisOptions) : base(servicoTelemetria)
        {
            this.connectionMultiplexerSME = connectionMultiplexerSME ?? throw new ArgumentNullException(nameof(connectionMultiplexerSME));
            redis = connectionMultiplexerSME.GetDatabase() ?? throw new ArgumentNullException("RedisDatabase");
            this.redisOptions = redisOptions ?? throw new ArgumentNullException(nameof(redisOptions));
            NomeServicoCache = "Cache Redis";
        }

        protected override string ObterValor(string nomeChave)
            => redis.StringGet(string.Concat(redisOptions.Prefixo, nomeChave));

        protected override Task RemoverValor(string nomeChave)
            => redis.KeyDeleteAsync(string.Concat(redisOptions.Prefixo, nomeChave));

        protected override Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar)
            => redis.StringSetAsync(new RedisKey(string.Concat(redisOptions.Prefixo, nomeChave)), new RedisValue(valor), TimeSpan.FromMinutes(minutosParaExpirar));
    }
}
