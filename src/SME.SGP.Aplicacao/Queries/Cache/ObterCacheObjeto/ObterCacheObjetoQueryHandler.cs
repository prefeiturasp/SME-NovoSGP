using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterCacheObjetoQueryHandler<T> : IRequestHandler<ObterCacheObjetoQuery<T>, T>
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterCacheObjetoQueryHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<T> Handle(ObterCacheObjetoQuery<T> request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(request.NomeCache, request.BuscarDados, request.MinutosParaExpirar,
                request.UtilizarGZip);
        }
    }
}