using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheAsyncQueryHandler : IRequestHandler<ObterCacheAsyncQuery,string>
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterCacheAsyncQueryHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<string> Handle(ObterCacheAsyncQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(request.NomeChave);
        }
    }
}