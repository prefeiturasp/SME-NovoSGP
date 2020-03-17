﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
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
                timer.Stop();

                servicoLog.RegistrarDependenciaAppInsights("Redis", nomeChave, $"Obtendo - Erro {ex.Message}", inicioOperacao, timer.Elapsed, false);
                return null;
            }
        }

        public T Obter<T>(string nomeChave)
        {
            try
            {
                var stringCache = distributedCache.GetString(nomeChave);
                if (!string.IsNullOrWhiteSpace(stringCache))
                    return JsonConvert.DeserializeObject<T>(stringCache);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
            }
            return default(T);
        }

        public async Task<T> Obter<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720)
        {
            try
            {
                var stringCache = distributedCache.GetString(nomeChave);
                if (!string.IsNullOrWhiteSpace(stringCache))
                    return JsonConvert.DeserializeObject<T>(stringCache);

                var dados = await buscarDados();
                await distributedCache.SetStringAsync(nomeChave, JsonConvert.SerializeObject(dados), new DistributedCacheEntryOptions()
                                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));
                return dados;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                return await buscarDados();
            }
        }

        public async Task<string> ObterAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var cacheParaRetorno = await distributedCache.GetStringAsync(nomeChave);
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
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await distributedCache.RemoveAsync(nomeChave);
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

        public void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720)
        {
            try
            {
                distributedCache.SetString(nomeChave, valor, new DistributedCacheEntryOptions()
                                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
            }
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                await distributedCache.SetStringAsync(nomeChave, valor, new DistributedCacheEntryOptions()
                                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));

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
        }
    }
}