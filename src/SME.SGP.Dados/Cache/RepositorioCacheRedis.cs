using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCacheRedis : RepositorioCache
    {
        private readonly IConnectionMultiplexerSME connectionMultiplexerSME;
        private readonly IDatabase redis;

        public RepositorioCacheRedis(IConnectionMultiplexerSME connectionMultiplexerSME, IServicoTelemetria servicoTelemetria) : base(servicoTelemetria)
        {
            this.connectionMultiplexerSME = connectionMultiplexerSME ?? throw new ArgumentNullException(nameof(connectionMultiplexerSME));
            this.redis = connectionMultiplexerSME.GetDatabase() ?? throw new ArgumentNullException("RedisDatabase");
            NomeServicoCache = "Cache Redis";
        }

        protected override string ObterValor(string nomeChave)
            => redis.StringGet(nomeChave);

        protected override Task RemoverValor(string nomeChave)
            => redis.KeyDeleteAsync(nomeChave);

        protected override Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar)
            => redis.StringSetAsync(new RedisKey(nomeChave), new RedisValue(valor), TimeSpan.FromMinutes(minutosParaExpirar));
    }
}
