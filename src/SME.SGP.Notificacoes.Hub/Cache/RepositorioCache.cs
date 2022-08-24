using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class RepositorioCache : IRepositorioCache
    {
        protected string NomeServicoCache { get; set; }

        protected virtual string ObterValor(string nomeChave)
            => throw new NotImplementedException($"Método ObterValor do serviço {NomeServicoCache} não implementado");

        protected virtual Task RemoverValor(string nomeChave)
            => throw new NotImplementedException($"Método RemoverValor do serviço {NomeServicoCache} não implementado");

        protected virtual Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar)
            => throw new NotImplementedException($"Método SalvarValor do serviço {NomeServicoCache} não implementado");

        public RepositorioCache()
        {
        }

        public string Obter(string nomeChave, string telemetriaNome, bool utilizarGZip = false)
        {
            var param = new
            {
                NomeChave = nomeChave,
                UtilizarGZip = utilizarGZip
            };
            
            var cacheParaRetorno = ObterValor(nomeChave);

            return cacheParaRetorno;            
        }
        
        public string Obter(string nomeChave, bool utilizarGZip = false)
        {
            return Obter(nomeChave, $"{NomeServicoCache} Obter", utilizarGZip);
        }        

        public async Task<string> ObterAsync(string nomeChave, string telemetriaNome, bool utilizarGZip = false)
        {
            var param = new
            {
                NomeChave = nomeChave,
                UtilizarGZip = utilizarGZip
            };
                
            var stringCache = ObterValor(nomeChave);

            if (string.IsNullOrWhiteSpace(stringCache)) 
                return await Task.FromResult(string.Empty);
            
            return stringCache;
        }
        
        public async Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false)
        {
            return await ObterAsync(nomeChave, $"{NomeServicoCache} Obter async<string>", utilizarGZip);
        }

        public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, string telemetriaNome, int minutosParaExpirar = 720,
            bool utilizarGZip = false)
        {
            var param = new
            {
                NomeChave = nomeChave,
                MinutosParaExpirar = minutosParaExpirar,
                UtilizarGZip = utilizarGZip
            };
            
            var stringCache = ObterValor(nomeChave);

            if (!string.IsNullOrWhiteSpace(stringCache))
            {
                return JsonConvert.DeserializeObject<T>(stringCache);
            }

            var dados = await buscarDados();

            await SalvarAsync(nomeChave, JsonConvert.SerializeObject(dados), minutosParaExpirar, utilizarGZip);

            return dados;            
        }
        
        public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720,
            bool utilizarGZip = false)
        {
            return await ObterAsync(nomeChave, buscarDados, $"{NomeServicoCache} Obter async<T>", minutosParaExpirar, utilizarGZip);
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var param = new
            {
                NomeChave = nomeChave
            };
            
            try
            {
                await RemoverValor(nomeChave);
            }
            catch (Exception)
            {
            }
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var param = new
            {
                NomeChave = nomeChave,
                MinutosParaExpirar = minutosParaExpirar,
                UtilizarGZip = utilizarGZip
            };
            
            try
            {
                if (!string.IsNullOrWhiteSpace(valor) && valor != "[]")
                {
                    await SalvarValor(nomeChave, valor, minutosParaExpirar);
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