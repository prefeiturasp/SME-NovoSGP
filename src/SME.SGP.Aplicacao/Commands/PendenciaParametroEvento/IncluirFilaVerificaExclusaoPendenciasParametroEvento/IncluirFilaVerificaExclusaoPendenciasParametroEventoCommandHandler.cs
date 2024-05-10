using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaVerificaExclusaoPendenciasParametroEventoCommandHandler : IRequestHandler<IncluirFilaVerificaExclusaoPendenciasParametroEventoCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaVerificaExclusaoPendenciasParametroEventoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaVerificaExclusaoPendenciasParametroEventoCommand request, CancellationToken cancellationToken)
        {
            return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaExclusaoPendenciaParametroEvento,
                                                       new VerificaExclusaoPendenciasParametroEventoCommand(request.TipoCalendarioId, request.UeCodigo, request.TipoEvento),
                                                       Guid.NewGuid(),
                                                       request.Usuario));
        }
    }
}
