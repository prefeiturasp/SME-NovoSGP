using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Cache;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public abstract class AtualizadorDeCache<T> where T : class
    {
        protected readonly IMediator mediator;
        protected ValorCache<T> ValorCache { get; set; }

        public AtualizadorDeCache(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected abstract T ObtenhaObjetoAtualizado(T objeto);

        protected abstract Task<ValorCache<T>> ObtenhaValorCache();

        protected async Task<bool> SalvaNoCache()
        {
            this.ValorCache = await ObtenhaValorCache();

            if (this.ValorCache.Valor != null)
            {
                var objetoCache = ObtenhaObjetoAtualizado(this.ValorCache.Valor);
                var cacheRetorno = JsonConvert.SerializeObject(objetoCache);

                await mediator.Send(new SalvarCachePorValorStringQuery(this.ValorCache.Chave, cacheRetorno));
            }

            return true;
        }
    }
}
