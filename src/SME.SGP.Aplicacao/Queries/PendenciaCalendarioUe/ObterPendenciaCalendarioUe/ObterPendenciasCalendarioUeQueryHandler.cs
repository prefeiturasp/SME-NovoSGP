using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasCalendarioUeQueryHandler : IRequestHandler<ObterPendenciasCalendarioUeQuery, IEnumerable<PendenciaCalendarioUe>>
    {
        private readonly IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe;

        public ObterPendenciasCalendarioUeQueryHandler(IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe)
        {
            this.repositorioPendenciaCalendarioUe = repositorioPendenciaCalendarioUe ?? throw new ArgumentNullException(nameof(repositorioPendenciaCalendarioUe));
        }


        public async Task<IEnumerable<PendenciaCalendarioUe>> Handle(ObterPendenciasCalendarioUeQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaCalendarioUe.ObterPendenciasPorCalendarioUe(request.TipoCalendarioId, request.UeId, request.TipoPendencia);
    }
}
