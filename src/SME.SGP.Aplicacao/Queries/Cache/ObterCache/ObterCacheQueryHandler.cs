using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheQueryHandler : IRequestHandler<ObterCacheQuery,string>
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterCacheQueryHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<string> Handle(ObterCacheQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(request.NomeChave);
        }
    }
}