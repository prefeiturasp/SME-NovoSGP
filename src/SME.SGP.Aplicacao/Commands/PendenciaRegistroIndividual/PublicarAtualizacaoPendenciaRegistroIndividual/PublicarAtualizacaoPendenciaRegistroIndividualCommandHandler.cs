using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PendenciaRegistroIndividual.PublicarAtualizacaoPendenciaRegistroIndividual
{
    public class PublicarAtualizacaoPendenciaRegistroIndividualCommandHandler : AsyncRequestHandler<PublicarAtualizacaoPendenciaRegistroIndividualCommand>
    {
        private readonly IMediator mediator;

        public PublicarAtualizacaoPendenciaRegistroIndividualCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected override async Task Handle(PublicarAtualizacaoPendenciaRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var dto = new AtualizarPendenciaRegistroIndividualDto { TurmaId = request.TurmaId, CodigoAluno = request.CodigoAluno, DataRegistro = request.DataRegistro };
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtualizarPendenciaAusenciaRegistroIndividual, dto, Guid.NewGuid(), null));
        }
    }
}