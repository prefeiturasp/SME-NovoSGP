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
    public class ObterPendenciaCalendarioUeQueryHandler : IRequestHandler<ObterPendenciaCalendarioUeQuery, PendenciaCalendarioUe>
    {
        private readonly IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe;

        public ObterPendenciaCalendarioUeQueryHandler(IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe)
        {
            this.repositorioPendenciaCalendarioUe = repositorioPendenciaCalendarioUe ?? throw new ArgumentNullException(nameof(repositorioPendenciaCalendarioUe));
        }


        public async Task<PendenciaCalendarioUe> Handle(ObterPendenciaCalendarioUeQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaCalendarioUe.ObterPendenciaPorCalendarioUe(request.TipoCalendarioId, request.UeId, request.TipoPendencia);
    }
}
