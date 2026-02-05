using SME.SGP.Dados.Cache;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Infra.Utilitarios;
using StackExchange.Redis;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCacheRedis : RepositorioCache
    {
        private readonly IDatabase _redis;
        private readonly RedisOptions _redisOptions;
        private readonly ICircuitBreaker _circuitBreaker;
        private readonly IMetricasCache _metricasCache;
        private readonly IServicoMensageriaLogs _servicoMensageriaLogs;

        public IConnectionMultiplexerSME ConnectionMultiplexerSme { get; }

        public RepositorioCacheRedis(
            IConnectionMultiplexerSME connectionMultiplexerSme,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaLogs servicoMensageriaLogs,
            IMetricasCache metricasCache,
            RedisOptions redisOptions,
            ICircuitBreaker circuitBreaker
        )
            : base(servicoTelemetria, servicoMensageriaLogs, metricasCache)
        {
            ConnectionMultiplexerSme =
                connectionMultiplexerSme ?? throw new ArgumentNullException(nameof(connectionMultiplexerSme));

            _redisOptions =
                redisOptions ?? throw new ArgumentNullException(nameof(redisOptions));

            _circuitBreaker =
                circuitBreaker ?? throw new ArgumentNullException(nameof(circuitBreaker));

            _redis = ConnectionMultiplexerSme.GetDatabase();

            NomeServicoCache = "Cache Redis";
            _metricasCache = metricasCache;
            _servicoMensageriaLogs = servicoMensageriaLogs;
        }

        protected override string ObterValor(string nomeChave)
        {
            if (!_circuitBreaker.PodeExecutar())
            {
                _metricasCache.Fail(nomeChave);
                return null;
            }

            var chave = MontarChave(nomeChave);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var valor = _redis.StringGet(chave);

                if (!valor.HasValue)
                {
                    _metricasCache.Miss(nomeChave);
                    return null;
                }

                _metricasCache.Hit(nomeChave);
                _circuitBreaker.RegistrarSucesso();

                return valor.ToString();
            }
            catch (RedisException ex)
            {
                stopwatch.Stop();
                _metricasCache.Fail(nomeChave);
                _circuitBreaker.RegistrarFalha();

                var mensagem = new LogMensagem($"Redis indisponível (GET). Cache ignorado",
                            LogNivel.Alerta.ToString(),
                            LogContexto.Cache.ToString(),
                            $"Nome chave: {nomeChave}",
                            "SGP",
                            ex.StackTrace,
                            ex.InnerException?.Message,
                            ex.InnerException?.ToString());

                _servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog").Wait();

                return null; 
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _metricasCache.Fail(nomeChave);
                _circuitBreaker.RegistrarFalha();

                var mensagem = new LogMensagem($"Erro inesperado no cache Redis",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    ex.StackTrace,
                    ex.InnerException?.Message,
                    ex.InnerException?.ToString());

                _servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog").Wait();
                return null;
            }
        }

        protected override async Task SalvarValor(
            string nomeChave,
            string valor,
            int minutosParaExpirar
        )
        {
            if (!_circuitBreaker.PodeExecutar())
                return;

            var chave = MontarChave(nomeChave);

            TimeSpan? expiry =
                minutosParaExpirar > 0
                    ? TimeSpan.FromMinutes(minutosParaExpirar)
                    : null;

            try
            {
                _ = _redis.StringSetAsync(
                    chave,
                    valor,
                    expiry,
                    flags: CommandFlags.FireAndForget
                );

                _circuitBreaker.RegistrarSucesso();
            }
            catch (RedisException ex)
            {
                _metricasCache.Fail(nomeChave);
                _circuitBreaker.RegistrarFalha();

                var mensagem = new LogMensagem($"Erro ao salvar valor no Redis",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    ex.StackTrace,
                    ex.InnerException?.Message,
                    ex.InnerException?.ToString());

                _servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog").Wait();
            }
        }

        protected override async Task RemoverValor(string nomeChave)
        {
            if (!_circuitBreaker.PodeExecutar())
                return;

            var chave = MontarChave(nomeChave);

            try
            {
                _ = _redis.KeyDeleteAsync(
                    chave,
                    flags: CommandFlags.FireAndForget
                );

                _circuitBreaker.RegistrarSucesso();
            }
            catch (RedisException ex)
            {
                _metricasCache.Fail(nomeChave);
                _circuitBreaker.RegistrarFalha();

                var mensagem = new LogMensagem($"Erro ao remover valor do Redis",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    ex.StackTrace,
                    ex.InnerException?.Message,
                    ex.InnerException?.ToString());

                _servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog").Wait();
            }
        }

        private string MontarChave(string nomeChave)
            => $"{_redisOptions.Prefixo}:{nomeChave}";
    }
}
