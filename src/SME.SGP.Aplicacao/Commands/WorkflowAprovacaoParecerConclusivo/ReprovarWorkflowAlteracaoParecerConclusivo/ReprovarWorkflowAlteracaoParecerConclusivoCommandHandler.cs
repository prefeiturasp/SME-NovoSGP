using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprovarWorkflowAlteracaoParecerConclusivoCommandHandler : AsyncRequestHandler<ReprovarWorkflowAlteracaoParecerConclusivoCommand>
    {
        private readonly IMediator mediator;

        public ReprovarWorkflowAlteracaoParecerConclusivoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ReprovarWorkflowAlteracaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var parecerEmAprovacao = await ObterParecerEmAprovacao(request.WorkflowId);
            if (parecerEmAprovacao != null)
                await ReprovarParecerConclusivo(parecerEmAprovacao, request);
        }

        private async Task ReprovarParecerConclusivo(WFAprovacaoParecerConclusivo parecerEmAprovacao, ReprovarWorkflowAlteracaoParecerConclusivoCommand request)
        {
            await ExcluirWorkFlow(parecerEmAprovacao);

            await NotificarAprovacao(parecerEmAprovacao, request);
        }

        private async Task NotificarAprovacao(WFAprovacaoParecerConclusivo parecerEmAprovacao, ReprovarWorkflowAlteracaoParecerConclusivoCommand request)
        {
            await mediator.Send(new NotificarAprovacaoParecerConclusivoCommand(parecerEmAprovacao,
                                                                               request.TurmaCodigo,
                                                                               request.CriadorRF,
                                                                               request.CriadorNome,
                                                                               false,
                                                                               request.Motivo));
        }

        private async Task ExcluirWorkFlow(WFAprovacaoParecerConclusivo parecerEmAprovacao)
        {
            await mediator.Send(new ExcluirWfAprovacaoParecerConclusivoCommand(parecerEmAprovacao.Id));
        }

        private async Task<WFAprovacaoParecerConclusivo> ObterParecerEmAprovacao(long workflowId)
            => await mediator.Send(new ObterParecerConclusivoEmAprovacaoPorWorkflowQuery(workflowId));

    }
}
