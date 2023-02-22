using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsPendenciaDevolutivaPorAnoLetivoQueryHandler : IRequestHandler<ObterIdsPendenciaDevolutivaPorAnoLetivoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaDevolutiva repositorioPendenciaDevolutiva;

        public ObterIdsPendenciaDevolutivaPorAnoLetivoQueryHandler(IRepositorioPendenciaDevolutiva repositorioPendenciaDevolutiva)
        {
            this.repositorioPendenciaDevolutiva = repositorioPendenciaDevolutiva ?? throw new ArgumentNullException(nameof(repositorioPendenciaDevolutiva));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsPendenciaDevolutivaPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioPendenciaDevolutiva.ObterIdsPendencias(request.AnoLetivo, request.CodigoUe);
        }
    }
}
