using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCache : IRepositorioCache
    {
        //private readonly Lazy<ConnectionMultiplexer> lazyConnection;

        //private readonly IDistributedCache distributedCache;
        private readonly IServicoLog servicoLog;

        public RepositorioCache(IServicoLog servicoLog)
        {
            //this.distributedCache = distributedCache ?? throw new System.ArgumentNullException(nameof(distributedCache));
            this.servicoLog = servicoLog ?? throw new System.ArgumentNullException(nameof(servicoLog));
            //this.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            //    {
            //        return ConnectionMultiplexer.Connect("localhost");
            //    });
        }

        public string Obter(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Lazy<ConnectionMultiplexer> lazyConnection = null;

            try
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect("localhost"); });

                //var cacheParaRetorno = distributedCache.GetString(nomeChave);
                var cacheParaRetorno = lazyConnection.Value.GetDatabase().StringGet(nomeChave);
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Obtendo", inicioOperacao, timer.Elapsed, true);

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
            finally
            {
                if (lazyConnection.Value != null)
                {
                    lazyConnection.Value.Close();
                    lazyConnection.Value.Dispose();
                    lazyConnection = null;
                }
            }
        }

        public T Obter<T>(string nomeChave)
        {
            Lazy<ConnectionMultiplexer> lazyConnection = null;

            try
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect("localhost"); });

                //var stringCache = distributedCache.GetString(nomeChave);

                var stringCache = lazyConnection.Value.GetDatabase().StringGet(nomeChave);

                if (!string.IsNullOrWhiteSpace(stringCache))
                    return JsonConvert.DeserializeObject<T>(stringCache);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
            }
            finally
            {
                if (lazyConnection.Value != null)
                {
                    lazyConnection.Value.Close();
                    lazyConnection.Value.Dispose();
                    lazyConnection = null;
                }
            }
            return default(T);
        }

        public async Task<T> Obter<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720)
        {
            Lazy<ConnectionMultiplexer> lazyConnection = null;

            try
            {
                //var stringCache = distributedCache.GetString(nomeChave);
                lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect("localhost"); });

                var stringCache = lazyConnection.Value.GetDatabase().StringGet(nomeChave);

                if (stringCache.HasValue)
                    return JsonConvert.DeserializeObject<T>(stringCache);

                //if (!string.IsNullOrWhiteSpace(stringCache))
                //    return JsonConvert.DeserializeObject<T>(stringCache);

                var dados = await buscarDados();

                await lazyConnection.Value.GetDatabase().StringSetAsync(nomeChave, JsonConvert.SerializeObject(dados), TimeSpan.FromMinutes(minutosParaExpirar));

                //await distributedCache.SetStringAsync(nomeChave, JsonConvert.SerializeObject(dados), new DistributedCacheEntryOptions()
                //                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));
                return dados;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                return await buscarDados();
            }
            finally
            {
                if (lazyConnection.Value != null)
                {
                    lazyConnection.Value.Close();
                    lazyConnection.Value.Dispose();
                    lazyConnection = null;
                }
            }
        }

        public async Task<string> ObterAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Lazy<ConnectionMultiplexer> lazyConnection = null;

            try
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect("localhost"); });
                //var cacheParaRetorno = await distributedCache.GetStringAsync(nomeChave);

                var cacheParaRetorno = await lazyConnection.Value.GetDatabase().StringGetAsync(nomeChave);

                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Obtendo async", inicioOperacao, timer.Elapsed, true);
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
            finally
            {
                if (lazyConnection.Value != null)
                {
                    lazyConnection.Value.Close();
                    lazyConnection.Value.Dispose();
                    lazyConnection = null;
                }
            }
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Lazy<ConnectionMultiplexer> lazyConnection = null;

            try
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect("localhost"); });

                //await distributedCache.RemoveAsync(nomeChave);
                await lazyConnection.Value.GetDatabase().KeyDeleteAsync(nomeChave);

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
            finally
            {
                if (lazyConnection.Value != null)
                {
                    lazyConnection.Value.Close();
                    lazyConnection.Value.Dispose();
                    lazyConnection = null;
                }
            }
        }

        public void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720)
        {
            Lazy<ConnectionMultiplexer> lazyConnection = null;

            try
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect("localhost"); });

                lazyConnection.Value.GetDatabase().StringSet(nomeChave, JsonConvert.SerializeObject(valor), TimeSpan.FromMinutes(minutosParaExpirar));
                //distributedCache.SetString(nomeChave, valor, new DistributedCacheEntryOptions()
                //                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
            }
            finally
            {
                if (lazyConnection.Value != null)
                {
                    lazyConnection.Value.Close();
                    lazyConnection.Value.Dispose();
                    lazyConnection = null;
                }
            }
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Lazy<ConnectionMultiplexer> lazyConnection = null;

            try
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect("localhost"); });

                await lazyConnection.Value.GetDatabase().StringSetAsync(nomeChave, valor, TimeSpan.FromMinutes(minutosParaExpirar));

                //await distributedCache.SetStringAsync(nomeChave, valor, new DistributedCacheEntryOptions()
                //                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));

                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Salvar async", inicioOperacao, timer.Elapsed, true);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Salvar async", inicioOperacao, timer.Elapsed, false);
                servicoLog.Registrar(ex);
            }
            finally
            {
                if (lazyConnection.Value != null)
                {
                    lazyConnection.Value.Close();
                    lazyConnection.Value.Dispose();
                    lazyConnection = null;
                }
            }
        }
    }
}