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
            var chave = ObterChave();
            var objeto = ObterDoCache(chave);

            if (objeto != null)
                return objeto;

            var objetoRepositorio = await ObterObjetoRepositorio();

            await repositorioCache.SalvarAsync(chave, JsonConvert.SerializeObject(objetoRepositorio));

            return objetoRepositorio;
        }

        private T ObterDoCache(string chave)
        {
            var cacheTurma = repositorioCache.Obter(chave);
            return cacheTurma != null ? JsonConvert.DeserializeObject<T>(cacheTurma) : null;
        }
    }
}
