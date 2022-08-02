using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public abstract class ServicoDeGerenciamentoDeCache<T> where T : class
    {
        private readonly IMediator mediator;

        public ServicoDeGerenciamentoDeCache(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected abstract T ObtenhaObjetoAtualizado();

        protected abstract string ObtenhaChave();

        public async Task<T> ObtenhaObjeto()
        {
            var cache = await mediator.Send(new ObterCacheQuery(ObtenhaChave()));

            if (!string.IsNullOrEmpty(cache))
                return JsonConvert.DeserializeObject<T>(cache);

            return null;
        }
        public async Task SalvaNoCache()
        {
            var objetoCache = ObtenhaObjetoAtualizado();

            if (objetoCache != null)
            {
                var cacheRetorno = JsonConvert.SerializeObject(objetoCache);
                await mediator.Send(new SalvarCachePorValorStringQuery(ObtenhaChave(), cacheRetorno));
            }
        }
    }
}
