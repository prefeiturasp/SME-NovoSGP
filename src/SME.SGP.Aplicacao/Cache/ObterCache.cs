using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public abstract class ObterCache<T> where T : class
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterCache(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        protected abstract string ObterChave();

        protected Task<ValorCache<T>> ObterDoCache()
        {
            var cacheTurma = repositorioCache.Obter(ObterChave());
            T valorCache = null;

            if (cacheTurma != null)
                valorCache = JsonConvert.DeserializeObject<T>(cacheTurma);

            var valor = new ValorCache<T>()
            {
                Chave = ObterChave(),
                Valor = valorCache
            };

            return Task.FromResult(valor);
        }
    }
}

