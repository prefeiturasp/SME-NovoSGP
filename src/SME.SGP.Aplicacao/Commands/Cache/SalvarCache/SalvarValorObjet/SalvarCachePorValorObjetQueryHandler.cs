using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class SalvarCachePorValorObjetQueryHandler : IRequestHandler<SalvarCachePorValorObjetQuery,string>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarCachePorValorObjetQueryHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<string> Handle(SalvarCachePorValorObjetQuery request, CancellationToken cancellationToken)
        {
            await repositorioCache.SalvarAsync(request.NomeChave,request.Valor,request.MinutosParaExpirar,request.UtilizarGZip);
            return request.NomeChave;
        }
    }
}