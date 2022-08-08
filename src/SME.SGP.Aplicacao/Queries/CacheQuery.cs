using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public abstract class CacheQuery<T> where T : class
    {
        private readonly IRepositorioCache repositorioCache;

        protected CacheQuery(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        protected abstract string ObterChave();
        protected abstract Task<T> ObterObjetoRepositorio();

        protected async Task<T> Obter()
        {
            return await repositorioCache.ObterAsync(ObterChave(), ObterObjetoRepositorio);
        }
    }
}
