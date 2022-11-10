using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra.Consts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AprovarWorkflowAlteracaoNotaConselhoCommandHandler : AsyncRequestHandler<AprovarWorkflowAlteracaoNotaConselhoCommand>
    {
        private readonly IMediator mediator;
        public readonly IUnitOfWork unitOfWork;

        public AprovarWorkflowAlteracaoNotaConselhoCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        protected override async Task Handle(AprovarWorkflowAlteracaoNotaConselhoCommand request, CancellationToken cancellationToken)
        {
            var notasEmAprovacao = await ObterNotasConselhoEmAprovacao(request.WorkflowId);
            if (notasEmAprovacao != null && notasEmAprovacao.ToArray().Any())
            {
                unitOfWork.IniciarTransacao();
                try
                {
                    foreach (var notaEmAprovacao in notasEmAprovacao)
                        await AtualizarNotaConselho(notaEmAprovacao, request);
                    unitOfWork.PersistirTransacao();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task AtualizarNotaConselho(WFAprovacaoNotaConselho notaEmAprovacao, AprovarWorkflowAlteracaoNotaConselhoCommand request)
        {
            var notaAnterior = notaEmAprovacao.ConselhoClasseNota.Nota;
            var conceitoAnterior = notaEmAprovacao.ConselhoClasseNota.ConceitoId;

            await AlterarNota(notaEmAprovacao);
            await ExcluirWorkFlow(notaEmAprovacao);

            await mediator.Send(new NotificarAprovacaoNotasConselhoCommand(new List<WFAprovacaoNotaConselho>(),
                                                                          request.CodigoDaNotificacao,
                                                                          request.TurmaCodigo,
                                                                          request.WorkflowId,
                                                                          true,
                                                                          ""));
            await GerarParecerConclusivo(notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno, notaEmAprovacao.UsuarioSolicitanteId);
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

            var periodoEscolar = notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar;

            var consolidacaoNotaAlunoDto = new ConsolidacaoNotaAlunoDto()
            {
                AlunoCodigo = notaConselhoClasse.ConselhoClasseAluno.AlunoCodigo,
                TurmaId = notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.Id,
                Bimestre = periodoEscolar == null ? (int?)null : periodoEscolar.Bimestre,
                AnoLetivo = notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar.Bimestre,
                Nota = notaConselhoClasse.Nota,
                ConceitoId = notaConselhoClasse.ConceitoId,
                ComponenteCurricularId = notaConselhoClasse.ComponenteCurricularCodigo
            };
            await mediator.Send(new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAlunoDto));
        }

        private async Task GerarParecerConclusivo(ConselhoClasseAluno conselhoClasseAluno, long usuarioSolicitanteId)
        {
            await mediator.Send(new GerarParecerConclusivoAlunoCommand(conselhoClasseAluno, usuarioSolicitanteId));
        }

        private async Task<IEnumerable<WFAprovacaoNotaConselho>> ObterNotasConselhoEmAprovacao(long workFlowId)
            => await mediator.Send(new ObterNotasConselhoEmAprovacaoPorWorkflowQuery(workFlowId));
    }
}
