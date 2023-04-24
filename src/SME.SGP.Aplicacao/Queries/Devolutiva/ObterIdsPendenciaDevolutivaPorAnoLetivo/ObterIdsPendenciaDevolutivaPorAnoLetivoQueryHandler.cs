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
        private readonly IRepositorioPendenciaDevolutiva repositorioPendenciaRegistroDevolutiva;

        public ObterIdsPendenciaDevolutivaPorAnoLetivoQueryHandler(IRepositorioPendenciaDevolutiva repositorioPendenciaRegistroDevolutiva)
        {
            this.repositorioPendenciaRegistroDevolutiva = repositorioPendenciaRegistroDevolutiva ?? throw new ArgumentNullException(nameof(repositorioPendenciaRegistroDevolutiva));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsPendenciaDevolutivaPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaRegistroDevolutiva.ObterIdsPendencias(request.AnoLetivo, request.CodigoUe);
        }
    }
}
