using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RecusarAprovacaoNotaConselhoCommandHandler : AsyncRequestHandler<RecusarAprovacaoNotaConselhoCommand>
    {
        private readonly IMediator mediator;

        public RecusarAprovacaoNotaConselhoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(RecusarAprovacaoNotaConselhoCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new ExcluirWfAprovacaoNotaConselhoClasseCommand(request.NotasEmAprovacao.Id));

            await mediator.Send(new NotificarAprovacaoNotaConselhoCommand(request.NotasEmAprovacao,
                                                                          request.CodigoDaNotificacao,
                                                                          request.TurmaCodigo,
                                                                          request.WorkflowId,
                                                                          false,
                                                                          request.Justificativa,
                                                                          request.NotaAnterior,
                                                                          request.ConceitoAnterior));

        }
    }
}

