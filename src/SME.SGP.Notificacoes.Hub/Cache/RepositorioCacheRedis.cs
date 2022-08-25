using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class RepositorioCacheRedis : RepositorioCache
    {
        private readonly IDatabase redis;
        private readonly RedisOptions redisOptions;

        public RepositorioCacheRedis(IConnectionMultiplexerSME connectionMultiplexerSME,
            RedisOptions redisOptions) : base()
        {
            var _connectionMultiplexerSME = connectionMultiplexerSME ?? throw new ArgumentNullException(nameof(connectionMultiplexerSME));
            redis = _connectionMultiplexerSME.GetDatabase() ?? throw new ArgumentNullException("RedisDatabase");
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
