using MediatR;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprovarWorkflowAlteracaoParecerConclusivoCommandHandler : IRequestHandler<ReprovarWorkflowAlteracaoParecerConclusivoCommand>
    {
        private readonly IMediator mediator;

        public ReprovarWorkflowAlteracaoParecerConclusivoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Handle(ReprovarWorkflowAlteracaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var pareceresEmAprovacao = await mediator.Send(new ObterPareceresConclusivosDtoEmAprovacaoPorWorkflowQuery(request.WorkflowId));
            if (pareceresEmAprovacao.NaoEhNulo() && pareceresEmAprovacao.Any())
            {
                foreach (var parecerEmAprovacao in pareceresEmAprovacao)
                    await mediator.Send(new ExcluirWfAprovacaoParecerConclusivoCommand(parecerEmAprovacao.Id));

                await mediator.Send(new NotificarAprovacaoParecerConclusivoCommand(pareceresEmAprovacao,
                                                                               request.TurmaCodigo,
                                                                               request.CriadorRF,
                                                                               request.CriadorNome,
                                                                               false,
                                                                               request.Motivo));
            }
        }


    }
}
