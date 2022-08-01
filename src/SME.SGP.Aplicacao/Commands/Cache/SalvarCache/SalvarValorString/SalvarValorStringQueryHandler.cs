using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class SalvarValorStringQueryHandler : IRequestHandler<SalvarValorStringQuery>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarValorStringQueryHandler(IRepositorioCache cache)
        {
            repositorioCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<Unit> Handle(SalvarValorStringQuery request, CancellationToken cancellationToken)
        {
            await repositorioCache.SalvarAsync(request.NomeChave, request.Valor,request.MinutosParaExpirar,request.UtilizarGZip);
            return Unit.Value;
        }
    }
}