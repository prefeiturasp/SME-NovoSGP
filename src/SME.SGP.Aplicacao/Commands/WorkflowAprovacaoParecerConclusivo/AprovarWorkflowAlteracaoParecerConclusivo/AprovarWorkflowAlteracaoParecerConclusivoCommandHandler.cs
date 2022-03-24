using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos;
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
            var parecerEmAprovacao = await mediator.Send(new ObterParecerConclusivoDtoEmAprovacaoPorWorkflowQuery(request.WorkflowId));
            if (parecerEmAprovacao != null)
                await AtualizarParecerConclusivo(parecerEmAprovacao, request.TurmaCodigo, request.CriadorRf, request.CriadorNome);
        }

        private async Task AtualizarParecerConclusivo(WFAprovacaoParecerConclusivoDto parecerEmAprovacao, string turmaCodigo, string criadorRf, string criadorNome)
        {
            var persistirParecerConclusivoDto = new PersistirParecerConclusivoDto()
            {
                ConselhoClasseAlunoId = parecerEmAprovacao.ConselhoClasseAlunoId,
                ConselhoClasseAlunoCodigo = parecerEmAprovacao.AlunoCodigo,
                ParecerConclusivoId = parecerEmAprovacao.ConselhoClasseParecerId,
                TurmaId = parecerEmAprovacao.TurmaId,
                TurmaCodigo = turmaCodigo,
                Bimestre = parecerEmAprovacao.Bimestre,
                AnoLetivo = parecerEmAprovacao.AnoLetivo
            };
            await mediator.Send(new PersistirParecerConclusivoCommand(persistirParecerConclusivoDto));
                        
            await mediator.Send(new ExcluirWfAprovacaoParecerConclusivoCommand(parecerEmAprovacao.WorkFlowAprovacaoId));

            await NotificarAprovacao(parecerEmAprovacao, turmaCodigo, criadorRf, criadorNome);
        }

        private async Task NotificarAprovacao(WFAprovacaoParecerConclusivoDto parecerEmAprovacao, string turmaCodigo, string criadorRf, string criadorNome)
        {
            await mediator.Send(new NotificarAprovacaoParecerConclusivoCommand(parecerEmAprovacao,
                                                                               turmaCodigo,
                                                                               criadorRf,
                                                                               criadorNome,
                                                                               true));
        }
    }
}
