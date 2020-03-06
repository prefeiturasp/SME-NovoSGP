using Microsoft.Extensions.Caching.Distributed;
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
            try
            {
                return distributedCache.GetString(nomeChave);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
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

        //public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados)
        //{
        //    try
        //    {
        //        var stringCache = await distributedCache.GetStringAsync(nomeChave);
        //        if (!string.IsNullOrWhiteSpace(stringCache))
        //            return JsonConvert.DeserializeObject<T>(stringCache);
        //        else
        //        {
        //            return await buscarDados();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
        //        servicoLog.Registrar(ex);
        //    }
        //    return default(T);
        //}

        public T Obter<T>(string nomeChave, Func<T> buscarDados, int minutosParaExpirar = 720)
        {
            try
            {
                var stringCache = distributedCache.GetString(nomeChave);
                if (!string.IsNullOrWhiteSpace(stringCache))
                    return JsonConvert.DeserializeObject<T>(stringCache);
                var dados = buscarDados();
                distributedCache.SetStringAsync(nomeChave, JsonConvert.SerializeObject(dados), new DistributedCacheEntryOptions()
                                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));
                return dados;
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                return buscarDados();
            }
        }

        public async Task<string> ObterAsync(string nomeChave)
        {
            try
            {
                return await distributedCache.GetStringAsync(nomeChave);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
                return null;
            }
        }

        public async Task RemoverAsync(string nomeChave)
        {
            try
            {
                await distributedCache.RemoveAsync(nomeChave);
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
            }
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720)
        {
            try
            {
                await distributedCache.SetStringAsync(nomeChave, valor, new DistributedCacheEntryOptions()
                                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));
            }
            catch (Exception ex)
            {
                //Caso o cache esteja indisponível a aplicação precisa continuar funcionando mesmo sem o cache
                servicoLog.Registrar(ex);
            }
        }
    }
}