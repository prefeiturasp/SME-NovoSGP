using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class SalvarValorObjetQueryHandler : IRequestHandler<SalvarValorObjetQuery>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarValorObjetQueryHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<Unit> Handle(SalvarValorObjetQuery request, CancellationToken cancellationToken)
        {
            await repositorioCache.SalvarAsync(request.NomeChave,request.Valor,request.MinutosParaExpirar,request.UtilizarGZip);
            return Unit.Value;
        }
    }
}