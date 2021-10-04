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

        private readonly IMemoryCache memoryCache;
        private readonly ServicoTelemetria servicoTelemetria;

        public RepositorioCache(IMemoryCache memoryCache, ServicoTelemetria servicoTelemetria)
        {
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
        }
        public string Obter(string nomeChave, bool utilizarGZip = false)
        {
            var cacheParaRetorno = servicoTelemetria.RegistrarComRetorno<string>(() => memoryCache.Get<string>(nomeChave), "Cache Obter", "", "");

            if (utilizarGZip)
            {
                cacheParaRetorno = UtilGZip.Descomprimir(Convert.FromBase64String(cacheParaRetorno));
            }

            return cacheParaRetorno;
        }
        public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var stringCache = servicoTelemetria.RegistrarComRetorno<string>(() => memoryCache.Get<string>(nomeChave), "Cache Obter async<T>", "", "");

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

        public async Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false)
        {
            var stringCache = servicoTelemetria.RegistrarComRetorno<string>(() => memoryCache.Get<string>(nomeChave), "Cache Obter async<T>", "Cache Obter async<T>", "");

            if (!string.IsNullOrWhiteSpace(stringCache))
            {
                if (utilizarGZip)
                {
                    stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));
                }
                return stringCache;
            }

            return await Task.FromResult(stringCache);
        }

        public async Task RemoverAsync(string nomeChave)
        {
            try
            {
                await Task.Factory.StartNew(() => servicoTelemetria.Registrar(() => memoryCache.Remove(nomeChave), "Cache Remover async", "Cache Remover async", ""));
            }
            catch (Exception)
            {
                                
            }            
        }

        public void Salvar(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            if (utilizarGZip)
            {
                var valorComprimido = UtilGZip.Comprimir(valor);
                valor = Convert.ToBase64String(valorComprimido);
            }
            try
            {
                servicoTelemetria.Registrar(() => memoryCache.Set(nomeChave, valor, TimeSpan.FromMinutes(minutosParaExpirar)), "Cache Obter async<T>", "", "");
            }
            catch (Exception)
            {
                                
            }
            
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(valor) && valor != "[]")
                {

                    if (utilizarGZip)
                    {
                        var valorComprimido = UtilGZip.Comprimir(valor);
                        valor = Convert.ToBase64String(valorComprimido);
                    }

                    await Task.Factory.StartNew(() => servicoTelemetria.Registrar(() => memoryCache.Set(nomeChave, valor, TimeSpan.FromMinutes(minutosParaExpirar)), "Cache Salvar async", "Cache Salvar async", ""));
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            await SalvarAsync(nomeChave, JsonConvert.SerializeObject(valor), minutosParaExpirar, utilizarGZip);
        }
    }
}