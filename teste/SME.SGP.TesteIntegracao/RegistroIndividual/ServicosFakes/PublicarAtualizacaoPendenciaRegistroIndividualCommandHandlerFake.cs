using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes
{
    public class PublicarAtualizacaoPendenciaRegistroIndividualCommandHandlerFake: AsyncRequestHandler<PublicarAtualizacaoPendenciaRegistroIndividualCommand>
    {
        private readonly IMediator mediator;

        public PublicarAtualizacaoPendenciaRegistroIndividualCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected override async Task Handle(PublicarAtualizacaoPendenciaRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var command = new AtualizarPendenciaRegistroIndividualDto { TurmaId = request.TurmaId, CodigoAluno = request.CodigoAluno, DataRegistro = request.DataRegistro };
            await mediator.Send(command);
        }
    }
}