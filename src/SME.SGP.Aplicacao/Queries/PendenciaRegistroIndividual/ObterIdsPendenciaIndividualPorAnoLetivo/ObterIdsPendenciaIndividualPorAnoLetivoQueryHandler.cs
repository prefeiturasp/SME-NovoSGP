using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsPendenciaIndividualPorAnoLetivoQueryHandler : IRequestHandler<ObterIdsPendenciaIndividualPorAnoLetivoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaRegistroIndividual repositorioPendenciaIndividual;

        public ObterIdsPendenciaIndividualPorAnoLetivoQueryHandler(IRepositorioPendenciaRegistroIndividual repositorioPendenciaIndividual)
        {
            this.repositorioPendenciaIndividual = repositorioPendenciaIndividual ?? throw new ArgumentNullException(nameof(repositorioPendenciaIndividual));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsPendenciaIndividualPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioPendenciaIndividual.ObterIdsPendencias(request.AnoLetivo, request.CodigoUe);
        }
    }
}
