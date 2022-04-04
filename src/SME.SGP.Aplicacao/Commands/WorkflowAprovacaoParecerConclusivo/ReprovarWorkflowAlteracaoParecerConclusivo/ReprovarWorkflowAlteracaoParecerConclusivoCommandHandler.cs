using MediatR;
using SME.SGP.Infra.Dtos;
using System;
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
            var parecerEmAprovacao = await mediator.Send(new ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQuery(request.WorkflowId));
            if (parecerEmAprovacao != null)
                await ReprovarParecerConclusivo(parecerEmAprovacao, request);
        }

        private async Task ReprovarParecerConclusivo(WFAprovacaoParecerConclusivoDto parecerEmAprovacao, ReprovarWorkflowAlteracaoParecerConclusivoCommand request)
        {
            await mediator.Send(new ExcluirWfAprovacaoParecerConclusivoCommand(parecerEmAprovacao.WorkFlowAprovacaoId));

            await NotificarAprovacao(parecerEmAprovacao, request);
        }

        private async Task NotificarAprovacao(WFAprovacaoParecerConclusivoDto parecerEmAprovacao, ReprovarWorkflowAlteracaoParecerConclusivoCommand request)
        {
            await mediator.Send(new NotificarAprovacaoParecerConclusivoCommand(parecerEmAprovacao,
                                                                               request.TurmaCodigo,
                                                                               request.CriadorRF,
                                                                               request.CriadorNome,
                                                                               false,
                                                                               request.Motivo));
        }


        private async Task<WFAprovacaoParecerConclusivoDto> ObterParecerEmAprovacao(long workflowId)
            => await mediator.Send(new ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQuery(workflowId));

    }
}
