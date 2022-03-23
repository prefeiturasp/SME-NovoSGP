using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AprovarWorkflowAlteracaoParecerConclusivoCommandHandler : AsyncRequestHandler<AprovarWorkflowAlteracaoParecerConclusivoCommand>
    {
        private readonly IMediator mediator;

        public AprovarWorkflowAlteracaoParecerConclusivoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(AprovarWorkflowAlteracaoParecerConclusivoCommand request, CancellationToken cancellationToken)
        {
            var parecerEmAprovacao = await ObterParecerEmAprovacao(request.WorkflowId);
            if (parecerEmAprovacao != null)
                await AtualizarParecerConclusivo(parecerEmAprovacao, request.TurmaCodigo, request.CriadorRf, request.CriadorNome);
        }

        private async Task AtualizarParecerConclusivo(WFAprovacaoParecerConclusivo parecerEmAprovacao, string turmaCodigo, string criadorRf, string criadorNome)
        {
            await AlterarParecer(parecerEmAprovacao);
            await ExcluirWorkFlow(parecerEmAprovacao);

            await NotificarAprovacao(parecerEmAprovacao, turmaCodigo, criadorRf, criadorNome);
        }

        private async Task NotificarAprovacao(WFAprovacaoParecerConclusivo parecerEmAprovacao, string turmaCodigo, string criadorRf, string criadorNome)
        {
            await mediator.Send(new NotificarAprovacaoParecerConclusivoCommand(parecerEmAprovacao,
                                                                               turmaCodigo,
                                                                               criadorRf,
                                                                               criadorNome,
                                                                               true));
        }

        private async Task AlterarParecer(WFAprovacaoParecerConclusivo parecerEmAprovacao)
        {
            var conselhoClasseAluno = parecerEmAprovacao.ConselhoClasseAluno;

            conselhoClasseAluno.ConselhoClasseParecerId = parecerEmAprovacao.ConselhoClasseParecerId;
            await mediator.Send(new SalvarConselhoClasseAlunoCommand(conselhoClasseAluno));
        }

        private async Task ExcluirWorkFlow(WFAprovacaoParecerConclusivo parecerEmAprovacao)
        {
            await mediator.Send(new ExcluirWfAprovacaoParecerConclusivoCommand(parecerEmAprovacao.Id));
        }

        private async Task<WFAprovacaoParecerConclusivo> ObterParecerEmAprovacao(long workflowId)
            => await mediator.Send(new ObterParecerConclusivoEmAprovacaoPorWorkflowQuery(workflowId));
    }
}
