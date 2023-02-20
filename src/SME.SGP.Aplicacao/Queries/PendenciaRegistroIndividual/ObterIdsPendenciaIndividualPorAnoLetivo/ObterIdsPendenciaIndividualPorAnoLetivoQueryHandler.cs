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
        private readonly IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual;

        public ObterIdsPendenciaIndividualPorAnoLetivoQueryHandler(IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual)
        {
            this.repositorioPendenciaRegistroIndividual = repositorioPendenciaRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioPendenciaRegistroIndividual));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsPendenciaIndividualPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioPendenciaRegistroIndividual.ObterIdsPendencias(request.AnoLetivo, request.CodigoUe);
        }
    }
}
