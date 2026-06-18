using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes
{
    public class PublicarAtualizacaoPendenciaRegistroIndividualCommandHandlerFake : IRequestHandler<PublicarAtualizacaoPendenciaRegistroIndividualCommand>
    {
        private readonly IMediator mediator;

        public PublicarAtualizacaoPendenciaRegistroIndividualCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Handle(PublicarAtualizacaoPendenciaRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var command = new AtualizarPendenciaRegistroIndividualDto { TurmaId = request.TurmaId, CodigoAluno = request.CodigoAluno, DataRegistro = request.DataRegistro };
            await mediator.Send(command);
        }
    }
}