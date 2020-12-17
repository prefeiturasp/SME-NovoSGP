using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCache : IRepositorioCache
    {

        private readonly IServicoLog servicoLog;
        private readonly IMemoryCache memoryCache;

        public RepositorioCache(IServicoLog servicoLog, IMemoryCache memoryCache)
        {

            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public string Obter(string nomeChave, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var cacheParaRetorno = memoryCache.Get<string>("nomeChave");
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Obtendo", inicioOperacao, timer.Elapsed, true);

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

                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, $"Obtendo - Erro {ex.Message}", inicioOperacao, timer.Elapsed, false);
                return null;
            }
        }

        public async Task<T> Obter<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var stringCache = memoryCache.Get<string>(nomeChave);

                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Obtendo", inicioOperacao, timer.Elapsed, true);

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
                servicoLog.Registrar(ex);
                timer.Stop();

                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, $"Obtendo - Erro {ex.Message}", inicioOperacao, timer.Elapsed, false);
                return default;
            }
        }



        public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var stringCache = memoryCache.Get<string>(nomeChave);

                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Obtendo", inicioOperacao, timer.Elapsed, true);

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
                servicoLog.Registrar(ex);
                timer.Stop();

                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, $"Obtendo - Erro {ex.Message}", inicioOperacao, timer.Elapsed, false);
                return default;
            }
        }

        public async Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var stringCache = memoryCache.Get<string>(nomeChave);

                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Obtendo", inicioOperacao, timer.Elapsed, true);

                if (!string.IsNullOrWhiteSpace(stringCache))
                {
                    if (utilizarGZip)
                    {
                        stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));
                    }
                    return stringCache;
                }

                return string.Empty;

            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                timer.Stop();

                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, $"Obtendo - Erro {ex.Message}", inicioOperacao, timer.Elapsed, false);
                return default;
            }
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                memoryCache.Remove(nomeChave);

                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Remover async", inicioOperacao, timer.Elapsed, true);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Remover async", inicioOperacao, timer.Elapsed, false);
                servicoLog.Registrar(ex);
            }
        }

        public void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                if (utilizarGZip)
                {
                    var valorComprimido = UtilGZip.Comprimir(valor);
                    valor = Convert.ToBase64String(valorComprimido);
                }

                memoryCache.Set(nomeChave, valor, TimeSpan.FromMinutes(minutosParaExpirar));

                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Remover async", inicioOperacao, timer.Elapsed, true);

            }
            catch (Exception ex)
            {
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Salvar", inicioOperacao, timer.Elapsed, false);
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

                    memoryCache.Set(nomeChave, valor, TimeSpan.FromMinutes(minutosParaExpirar));

                    timer.Stop();
                    servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Remover async", inicioOperacao, timer.Elapsed, true);
                }
            }
            catch (Exception ex)
            {
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("MemoryCache", nomeChave, "Salvar", inicioOperacao, timer.Elapsed, false);
                servicoLog.Registrar(ex);
            }
        }

        public async Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            await SalvarAsync(nomeChave, JsonConvert.SerializeObject(valor), minutosParaExpirar, utilizarGZip);
        }
    }
}