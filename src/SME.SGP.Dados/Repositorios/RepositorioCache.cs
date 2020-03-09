using Microsoft.Extensions.Caching.Distributed;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCache : IRepositorioCache
    {
        private readonly IDistributedCache distributedCache;
        private readonly IServicoLog servicoLog;

        public RepositorioCache(IDistributedCache distributedCache, IServicoLog servicoLog)
        {
            this.distributedCache = distributedCache ?? throw new System.ArgumentNullException(nameof(distributedCache));
            this.servicoLog = servicoLog ?? throw new System.ArgumentNullException(nameof(servicoLog));
        }

        public string Obter(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var cacheParaRetorno = distributedCache.GetString(nomeChave);
                timer.Stop();
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Obtendo", inicioOperacao, timer.Elapsed, true);

                return cacheParaRetorno;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                var fimOperacao = DateTime.Now;
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, $"Obtendo - Erro {ex.Message}", inicioOperacao, inicioOperacao - fimOperacao, false);
                return null;
            }
        }

        public async Task<string> ObterAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.Now;

            try
            {
                var cacheParaRetorno = await distributedCache.GetStringAsync(nomeChave);
                var fimOperacao = DateTime.Now;
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Obtendo async", inicioOperacao, inicioOperacao - fimOperacao, true);
                return cacheParaRetorno;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                var fimOperacao = DateTime.Now;
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, $"Obtendo async - Erro {ex.Message}", inicioOperacao, inicioOperacao - fimOperacao, false);
                return null;
            }
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.Now;

            try
            {
                await distributedCache.RemoveAsync(nomeChave);
                var fimOperacao = DateTime.Now;
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Remover async", inicioOperacao, inicioOperacao - fimOperacao, true);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                var fimOperacao = DateTime.Now;
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Remover async", inicioOperacao, inicioOperacao - fimOperacao, false);
                servicoLog.Registrar(ex);
            }
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720)
        {
            var inicioOperacao = DateTime.Now;

            try
            {
                await distributedCache.SetStringAsync(nomeChave, valor, new DistributedCacheEntryOptions()
                                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));

                var fimOperacao = DateTime.Now;
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Salvar async", inicioOperacao, inicioOperacao - fimOperacao, true);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                var fimOperacao = DateTime.Now;
                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, "Salvar async", inicioOperacao, inicioOperacao - fimOperacao, false);
                servicoLog.Registrar(ex);
            }
        }
    }
}