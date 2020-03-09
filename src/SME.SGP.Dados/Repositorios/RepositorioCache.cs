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