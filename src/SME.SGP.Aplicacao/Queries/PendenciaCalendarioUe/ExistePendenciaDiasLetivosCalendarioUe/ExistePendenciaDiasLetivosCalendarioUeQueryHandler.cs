using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaDiasLetivosCalendarioUeQueryHandler : IRequestHandler<ExistePendenciaDiasLetivosCalendarioUeQuery, bool>
    {
        private readonly IMediator mediator;

        public ExistePendenciaDiasLetivosCalendarioUeQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExistePendenciaDiasLetivosCalendarioUeQuery request, CancellationToken cancellationToken)
        {
            var pendenciaUe = await mediator.Send(new ObterPendenciaCalendarioUeQuery(request.TipoCalendarioId, request.UeId, Dominio.TipoPendencia.CalendarioLetivoInsuficiente));
            return pendenciaUe != null;
        }
    }
}
