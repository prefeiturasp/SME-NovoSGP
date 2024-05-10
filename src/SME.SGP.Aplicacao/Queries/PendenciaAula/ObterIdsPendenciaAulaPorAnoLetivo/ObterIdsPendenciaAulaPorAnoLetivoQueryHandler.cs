using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsPendenciaAulaPorAnoLetivoQueryHandler : IRequestHandler<ObterIdsPendenciaAulaPorAnoLetivoQuery, IEnumerable<long>>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterIdsPendenciaAulaPorAnoLetivoQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsPendenciaAulaPorAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioPendenciaAula.ObterIdsPendencias(request.AnoLetivo, request.CodigoUe);
        }
    }
}
