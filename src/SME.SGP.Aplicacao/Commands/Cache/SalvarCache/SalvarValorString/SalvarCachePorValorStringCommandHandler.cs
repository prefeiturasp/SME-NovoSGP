using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class SalvarCachePorValorStringCommandHandler : IRequestHandler<SalvarCachePorValorStringCommand>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarCachePorValorStringCommandHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<Unit> Handle(SalvarCachePorValorStringCommand request, CancellationToken cancellationToken)
        {
            await repositorioCache.SalvarAsync(request.NomeChave, request.Valor,request.MinutosParaExpirar,request.UtilizarGZip);
            return Unit.Value;
        }
    }
}