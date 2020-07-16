using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCache : IRepositorioCache
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;
        private readonly IServicoLog servicoLog;

        public RepositorioCache(IConnectionMultiplexer connectionMultiplexer, IServicoLog servicoLog)
        {
            this.connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public string Obter(string nomeChave, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var cacheParaRetorno = connectionMultiplexer.GetDatabase().StringGet(nomeChave);
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Obtendo", inicioOperacao, timer.Elapsed, true);

                if (utilizarGZip)
                {
                    cacheParaRetorno = UtilGZip.Descomprimir(Convert.FromBase64String(cacheParaRetorno));
                }

                return cacheParaRetorno;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                timer.Stop();

                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, $"Obtendo - Erro {ex.Message}", inicioOperacao, timer.Elapsed, false);
                return null;
            }
        }

        public T Obter<T>(string nomeChave, bool utilizarGZip = false)
        {
            try
            {
                var stringCache = connectionMultiplexer.GetDatabase().StringGet(nomeChave).ToString();
                if (!string.IsNullOrWhiteSpace(stringCache))
                {
                    if (utilizarGZip)
                    {
                        stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));
                    }
                    return JsonConvert.DeserializeObject<T>(stringCache);
                }
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
            }
            return default(T);
        }

        public async Task<T> Obter<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            try
            {
                var stringCache = connectionMultiplexer.GetDatabase().StringGet(nomeChave).ToString();
                if (!string.IsNullOrWhiteSpace(stringCache))
                {
                    if (utilizarGZip)
                    {
                        stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));
                    }
                    return JsonConvert.DeserializeObject<T>(stringCache);
                }

                var dados = await buscarDados();

                await SalvarAsync(nomeChave, JsonConvert.SerializeObject(dados), minutosParaExpirar, utilizarGZip);

                return dados;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                return await buscarDados();
            }
        }

        public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var stringCache = await connectionMultiplexer.GetDatabase().StringGetAsync(nomeChave);

                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Obtendo Async", inicioOperacao, timer.Elapsed, true);

                if (!string.IsNullOrWhiteSpace(stringCache.ToString()))
                {
                    if (utilizarGZip)
                    {
                        stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));
                    }
                    return JsonConvert.DeserializeObject<T>(stringCache);
                }

                var dados = await buscarDados();

                await SalvarAsync(nomeChave, dados, minutosParaExpirar, utilizarGZip);

                return dados;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, $"Obtendo Async - Erro {ex.Message}", inicioOperacao, timer.Elapsed, false);

                servicoLog.Registrar(ex);
                return await buscarDados();
            }
        }

        public async Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var cacheParaRetorno = await connectionMultiplexer.GetDatabase().StringGetAsync(nomeChave);
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Obtendo async", inicioOperacao, timer.Elapsed, true);

                if (!string.IsNullOrEmpty(cacheParaRetorno.ToString()) && utilizarGZip)
                {
                    cacheParaRetorno = UtilGZip.Descomprimir(Convert.FromBase64String(cacheParaRetorno));
                }

                return cacheParaRetorno;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, $"Obtendo async - Erro {ex.Message}", inicioOperacao, timer.Elapsed, false);
                return null;
            }
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await connectionMultiplexer.GetDatabase().KeyDeleteAsync(nomeChave);
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Remover async", inicioOperacao, timer.Elapsed, true);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Remover async", inicioOperacao, timer.Elapsed, false);
                servicoLog.Registrar(ex);
            }
        }

        public void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            try
            {
                if (utilizarGZip)
                {
                    var valorComprimido = UtilGZip.Comprimir(valor);
                    valor = Convert.ToBase64String(valorComprimido);
                }

                var redisKey = new RedisKey(nomeChave);
                var rediValue = new RedisValue(valor);

                connectionMultiplexer.GetDatabase()
                    .StringSet(redisKey, rediValue, TimeSpan.FromMinutes(minutosParaExpirar));
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
            }
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                if (!string.IsNullOrWhiteSpace(valor) && valor != "[]")
                {
                    if (utilizarGZip)
                    {
                        var valorComprimido = UtilGZip.Comprimir(valor);
                        valor = Convert.ToBase64String(valorComprimido);
                    }
                    await connectionMultiplexer.GetDatabase()
                        .StringSetAsync(nomeChave, valor, TimeSpan.FromMinutes(minutosParaExpirar));

                    timer.Stop();
                    servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Salvar async", inicioOperacao, timer.Elapsed, true);
                }
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Salvar async", inicioOperacao, timer.Elapsed, false);
                servicoLog.Registrar(ex);
            }
        }

        public async Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            await SalvarAsync(nomeChave, JsonConvert.SerializeObject(valor), minutosParaExpirar, utilizarGZip);
        }
    }
}