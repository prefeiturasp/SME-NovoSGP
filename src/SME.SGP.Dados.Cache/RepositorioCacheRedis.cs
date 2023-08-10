using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCacheRedis : RepositorioCache
    {
        private readonly IDatabase redis;
        private readonly RedisOptions redisOptions;

        public RepositorioCacheRedis(IConnectionMultiplexerSME connectionMultiplexerSme,
                                     IServicoTelemetria servicoTelemetria,
                                     RedisOptions redisOptions,
                                     IServicoMensageriaLogs servicoMensageriaLogs) 
            : base(servicoTelemetria, servicoMensageriaLogs)
        {
            ConnectionMultiplexerSme = connectionMultiplexerSme ?? throw new ArgumentNullException(nameof(connectionMultiplexerSme));
            redis = connectionMultiplexerSme.GetDatabase() ?? throw new ArgumentNullException(nameof(connectionMultiplexerSme.GetDatabase), "RedisDatabase");
            this.redisOptions = redisOptions ?? throw new ArgumentNullException(nameof(redisOptions));
            NomeServicoCache = "Cache Redis";
        }
        
        public IConnectionMultiplexerSME ConnectionMultiplexerSme { get; }

        protected override string ObterValor(string nomeChave)
            => redis.StringGet(string.Concat(redisOptions.Prefixo, nomeChave));

        protected override Task RemoverValor(string nomeChave)
            => redis.KeyDeleteAsync(string.Concat(redisOptions.Prefixo, nomeChave));

        protected override Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar)
            => redis.StringSetAsync(new RedisKey(string.Concat(redisOptions.Prefixo, nomeChave)), new RedisValue(valor), TimeSpan.FromMinutes(minutosParaExpirar));
    }
}
