using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AprovarWorkflowAlteracaoNotaConselhoCommandHandler : AsyncRequestHandler<AprovarWorkflowAlteracaoNotaConselhoCommand>
    {
        private readonly IMediator mediator;

        public AprovarWorkflowAlteracaoNotaConselhoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(AprovarWorkflowAlteracaoNotaConselhoCommand request, CancellationToken cancellationToken)
        {
            var notaEmAprovacao = await ObterNotaConselhoEmAprovacao(request.WorkflowId);
            if (notaEmAprovacao != null)
                await AtualizarNotaConselho(notaEmAprovacao, request);
        }

        private async Task AtualizarNotaConselho(WFAprovacaoNotaConselho notaEmAprovacao, AprovarWorkflowAlteracaoNotaConselhoCommand request)
        {
            var notaAnterior = notaEmAprovacao.ConselhoClasseNota.Nota;
            var conceitoAnterior = notaEmAprovacao.ConselhoClasseNota.ConceitoId;

            await AlterarNota(notaEmAprovacao);
            await ExcluirWorkFlow(notaEmAprovacao);

            await mediator.Send(new NotificarAprovacaoNotaConselhoCommand(notaEmAprovacao,
                                                                          request.CodigoDaNotificacao,
                                                                          request.TurmaCodigo,
                                                                          request.WorkflowId,
                                                                          true,
                                                                          "",
                                                                          notaAnterior,
                                                                          conceitoAnterior));
            await GerarParecerConclusivo(notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno);
        }

        private async Task ExcluirWorkFlow(WFAprovacaoNotaConselho notaEmAprovacao)
        {
            await mediator.Send(new ExcluirWfAprovacaoNotaConselhoClasseCommand(notaEmAprovacao.Id));
        }

        private async Task AlterarNota(WFAprovacaoNotaConselho notaEmAprovacao)
        {
            var notaConselhoClasse = notaEmAprovacao.ConselhoClasseNota;

            notaConselhoClasse.Nota = notaEmAprovacao.Nota;
            notaConselhoClasse.ConceitoId = notaEmAprovacao.ConceitoId;

            await mediator.Send(new SalvarConselhoClasseNotaCommand(notaConselhoClasse));
        }

        private async Task GerarParecerConclusivo(ConselhoClasseAluno conselhoClasseAluno)
        {
            await mediator.Send(new GerarParecerConclusivoAlunoCommand(conselhoClasseAluno));
        }

        private async Task<WFAprovacaoNotaConselho> ObterNotaConselhoEmAprovacao(long workFlowId)
            => await mediator.Send(new ObterNotaConselhoEmAprovacaoPorWorkflowQuery(workFlowId));
    }
}
