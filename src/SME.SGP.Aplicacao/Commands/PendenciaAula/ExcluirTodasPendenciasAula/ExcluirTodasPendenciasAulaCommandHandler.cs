using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirTodasPendenciasAulaCommandHandler : IRequestHandler<ExcluirTodasPendenciasAulaCommand, bool>
    {
        private readonly IMediator mediator;

        public ExcluirTodasPendenciasAulaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirTodasPendenciasAulaCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.Frequencia));
            await mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.PlanoAula));
            await mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.DiarioBordo));
            await mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.Avaliacao));
            await mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.AulaNaoLetivo));

            return true;
        }
    }
}
