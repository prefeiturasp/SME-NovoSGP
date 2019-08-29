using Microsoft.Extensions.Caching.Distributed;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCache : IRepositorioCache
    {
        private readonly IDistributedCache distributedCache;

        public RepositorioCache(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache ?? throw new System.ArgumentNullException(nameof(distributedCache));
        }

        public string Obter(string nomeChave)
        {
            return distributedCache.GetString(nomeChave);
        }

        public async Task SalvarAsync(string nomeChave, string valor)
        {
            await distributedCache.SetStringAsync(nomeChave, valor);
        }
    }
}