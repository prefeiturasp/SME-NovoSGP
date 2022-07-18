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
        private readonly IServicoTelemetria servicoTelemetria;

        protected string NomeServicoCache { get; set; }

        protected virtual string ObterValor(string nomeChave)
            => string.Empty;

        protected virtual Task RemoverValor(string nomeChave) 
            => throw new NotImplementedException($"Método RemoverValor do serviço {NomeServicoCache} não implementado");

        protected virtual Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar) 
            => throw new NotImplementedException($"Método SalvarValor do serviço {NomeServicoCache} não implementado");

        public RepositorioCache(IServicoTelemetria servicoTelemetria)
        {
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
        }

        public string Obter(string nomeChave, bool utilizarGZip = false)
        {
            var cacheParaRetorno = servicoTelemetria.RegistrarComRetorno<string>(() => ObterValor(nomeChave), NomeServicoCache, $"{NomeServicoCache} Obter", "");

            if (utilizarGZip)
            {
                cacheParaRetorno = UtilGZip.Descomprimir(Convert.FromBase64String(cacheParaRetorno));
            }

            return cacheParaRetorno;
        }

        public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var stringCache = servicoTelemetria.RegistrarComRetorno<string>(() => ObterValor(nomeChave), NomeServicoCache, $"{NomeServicoCache} Obter async<T>", "");

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
            var stringCache = servicoTelemetria.RegistrarComRetorno<string>(() => ObterValor(nomeChave), NomeServicoCache, $"{NomeServicoCache} Obter async<T>", "");

            if (!string.IsNullOrWhiteSpace(stringCache))
            {
                if (utilizarGZip)
                {
                    stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));
                }
                return stringCache;
            }

            return await Task.FromResult(string.Empty);
        }

        public async Task RemoverAsync(string nomeChave)
        {
            try
            {
                await servicoTelemetria.RegistrarAsync(async () => await RemoverValor(nomeChave), NomeServicoCache, $"{NomeServicoCache} Remover async", "");
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

                    await servicoTelemetria.RegistrarAsync(async () => await SalvarValor(nomeChave, valor, minutosParaExpirar), NomeServicoCache, $"{NomeServicoCache} Salvar async", "");
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