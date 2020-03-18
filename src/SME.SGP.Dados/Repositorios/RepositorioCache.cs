using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;
        private readonly IServicoLog servicoLog;

        public RepositorioCache(IServicoLog servicoLog, IConfiguration configuration)
        {
            this.servicoLog = servicoLog ?? throw new System.ArgumentNullException(nameof(servicoLog));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Obter(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Lazy<ConnectionMultiplexer> conexao = null;

            try
            {
                conexao = ObterConexao();

                //var cacheParaRetorno = distributedCache.GetString(nomeChave);
                var cacheParaRetorno = conexao.Value.GetDatabase().StringGet(ObterChaveInstancia(nomeChave));
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
                Dispose(conexao);
            }
        }

        public T Obter<T>(string nomeChave)
        {
            Lazy<ConnectionMultiplexer> conexao = null;

            try
            {
                conexao = ObterConexao();

                //var stringCache = distributedCache.GetString(nomeChave);

                var stringCache = conexao.Value.GetDatabase().StringGet(ObterChaveInstancia(nomeChave));

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
                Dispose(conexao);
            }
            return default(T);
        }

        public async Task<T> Obter<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720)
        {
            Lazy<ConnectionMultiplexer> conexao = null;

            try
            {
                conexao = ObterConexao();

                var stringCache = conexao.Value.GetDatabase().StringGet(ObterChaveInstancia(nomeChave));

                if (stringCache.HasValue)
                    return JsonConvert.DeserializeObject<T>(stringCache);

                var dados = await buscarDados();

                await conexao.Value.GetDatabase().StringSetAsync(ObterChaveInstancia(nomeChave), JsonConvert.SerializeObject(dados), TimeSpan.FromMinutes(minutosParaExpirar));

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
                Dispose(conexao);
            }
        }

        public async Task<string> ObterAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Lazy<ConnectionMultiplexer> conexao = null;

            try
            {
                conexao = ObterConexao();

                var cacheParaRetorno = await conexao.Value.GetDatabase().StringGetAsync(ObterChaveInstancia(nomeChave));

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
                Dispose(conexao);
            }
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Lazy<ConnectionMultiplexer> conexao = null;

            try
            {
                conexao = ObterConexao();

                await conexao.Value.GetDatabase().KeyDeleteAsync(ObterChaveInstancia(nomeChave));

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
                Dispose(conexao);
            }
        }

        public void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720)
        {
            Lazy<ConnectionMultiplexer> conexao = null;

            try
            {
                conexao = ObterConexao();

                conexao.Value.GetDatabase().StringSet(ObterChaveInstancia(nomeChave), JsonConvert.SerializeObject(valor), TimeSpan.FromMinutes(minutosParaExpirar));
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
                Dispose(conexao);
            }
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Lazy<ConnectionMultiplexer> conexao = null;

            try
            {
                conexao = ObterConexao();

                await conexao.Value.GetDatabase().StringSetAsync(ObterChaveInstancia(nomeChave), valor, TimeSpan.FromMinutes(minutosParaExpirar));

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
                Dispose(conexao);
            }
        }

        private void Dispose(Lazy<ConnectionMultiplexer> conexao)
        {
            if (conexao != null && conexao.IsValueCreated)
            {
                conexao.Value.Close();
                conexao.Value.Dispose();
            }
        }

        private string ObterChaveInstancia(string nomeChave)
        {
            return configuration.GetValue<string>("Nome-Instancia-Redis") + nomeChave;
        }

        private Lazy<ConnectionMultiplexer> ObterConexao()
        {
            return new Lazy<ConnectionMultiplexer>(() => { return ConnectionMultiplexer.Connect($"{configuration.GetConnectionString("SGP-Redis")},allowAdmin=true,syncTimeout=1000,connectTimeout=1000"); });
        }
    }
}