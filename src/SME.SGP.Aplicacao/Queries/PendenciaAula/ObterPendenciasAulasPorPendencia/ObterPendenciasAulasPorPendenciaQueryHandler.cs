using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAulasPorPendenciaQueryHandler : IRequestHandler<ObterPendenciasAulasPorPendenciaQuery, IEnumerable<PendenciaAulaDto>>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;

        public ObterPendenciasAulasPorPendenciaQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<IEnumerable<PendenciaAulaDto>> Handle(ObterPendenciasAulasPorPendenciaQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.ObterPendenciasAulasPorPendencia(request.PendenciaId);
    }
}
