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
    public class ExcluirPendenciaParametroEventoCommandHandler : IRequestHandler<ExcluirPendenciaParametroEventoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento;

        public ExcluirPendenciaParametroEventoCommandHandler(IMediator mediator, IRepositorioPendenciaParametroEvento repositorioPendenciaParametroEvento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaParametroEvento = repositorioPendenciaParametroEvento ?? throw new ArgumentNullException(nameof(repositorioPendenciaParametroEvento));
        }


        public async Task<bool> Handle(ExcluirPendenciaParametroEventoCommand request, CancellationToken cancellationToken)
        {
            repositorioPendenciaParametroEvento.Remover(request.PendenciaParametroEvento);

            var pendenciasParametrosRestantes = await mediator.Send(new ObterPendenciasParametroEventoPorPendenciaCalendarioUeQuery(request.PendenciaParametroEvento.PendenciaCalendarioUeId));
            if (pendenciasParametrosRestantes == null)
                await mediator.Send(new ExcluirPendenciaCalendarioUeCommand(request.PendenciaParametroEvento.PendenciaCalendarioUe.TipoCalendarioId, request.PendenciaParametroEvento.PendenciaCalendarioUe.UeId, TipoPendencia.CadastroEventoPendente));

            return true;
        }
    }
}
