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
            var exclusaoPendenciaFrequencia = mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.Frequencia));
            var exclusaoPendenciaPlanoAula = mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.PlanoAula));
            var exclusaoPendenciaDiarioBordo = mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.DiarioBordo));
            var exclusaoPendenciaAvaliacao = mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.Avaliacao));
            var exclusaoPendenciaAulaNaoLetivo = mediator.Send(new ExcluirPendenciaAulaCommand(request.AulaId, Dominio.TipoPendencia.AulaNaoLetivo));

            return await exclusaoPendenciaFrequencia
                && await exclusaoPendenciaPlanoAula
                && await exclusaoPendenciaDiarioBordo
                && await exclusaoPendenciaAvaliacao
                && await exclusaoPendenciaAulaNaoLetivo;
        }
    }
}
