using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.PendenciaAula;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PendenciaAula.ObterPendenciasAulasPorPendenciaIds
{
    class ObterPendenciasAulasPorPendenciaIdsQueryHandler : IRequestHandler<ObterPendenciasAulasPorPendenciaIdsQuery, IEnumerable<PendenciaAulasDto>>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciasAulasPorPendenciaIdsQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<IEnumerable<PendenciaAulasDto>> Handle(ObterPendenciasAulasPorPendenciaIdsQuery request, CancellationToken cancellationToken)
         => await repositorioPendenciaAula.ObterPendenciasAulasPorPendenciaIds(request.PendenciaIds);
    }
}
