using MediatR;
using SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDataEhDiaLetivoPorTipoCalendarioQueryHandler : IRequestHandler<ObterDataEhDiaLetivoPorTipoCalendarioQuery, bool>
    {
        private readonly IMediator mediator;

        public ObterDataEhDiaLetivoPorTipoCalendarioQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(ObterDataEhDiaLetivoPorTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(request.TipoCalendarioId, request.Data.Date));
            if (periodoEscolar == null)
                return false;

            var ehEventoNaoLetivo = await mediator.Send(new ObterTemEventoNaoLetivoPorCalendarioEDiaQuery(request.TipoCalendarioId, request.Data, request.CodigoDre, request.CodigoUe));
            if (ehEventoNaoLetivo)
                return false;

            var possuiLiberacaoExcepcional = await mediator.Send(new ObterDataPossuiEventoLiberacaoExcepcionalQuery(request.Data, request.TipoCalendarioId, request.CodigoUe));
            return possuiLiberacaoExcepcional || !request.Data.FimDeSemana();
        }
    }
}
