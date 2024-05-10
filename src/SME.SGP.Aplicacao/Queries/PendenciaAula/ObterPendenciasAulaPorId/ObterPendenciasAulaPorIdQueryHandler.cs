using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulaPorIdQueryHandler : IRequestHandler<ObterPendenciasAulaPorIdQuery, IEnumerable<PendenciaAulaDto>>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciasAulaPorIdQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }
        public async Task<IEnumerable<PendenciaAulaDto>> Handle(ObterPendenciasAulaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaAula.ObterPendenciasAulasPorPendencia(request.PendenciaId);
        }
    }
}