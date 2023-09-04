using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dto;
using System;
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

                    await mediator.Send(new NotificarAprovacaoNotasConselhoCommand(notasEmAprovacao,
                                                                          request.CodigoDaNotificacao,
                                                                          request.TurmaCodigo,
                                                                          request.WorkflowId,
                                                                          true,
                                                                          ""));
                    unitOfWork.PersistirTransacao();

                    var periodoEscolar = notasEmAprovacao.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar;
                    var bimestre = periodoEscolar != null ? periodoEscolar.Bimestre : (int)Bimestre.Final;
                    var codigoAluno = notasEmAprovacao.FirstOrDefault().ConselhoClasseNota.ConselhoClasseAluno.AlunoCodigo;
                    await RemoverCache(string.Format(NomeChaveCache.NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE_ALUNO, request.TurmaCodigo, bimestre, codigoAluno), cancellationToken);
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task RemoverCache(string nomeChave, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoverChaveCacheCommand(nomeChave), cancellationToken);
        }

        private async Task AtualizarNotaConselho(WFAprovacaoNotaConselho notaEmAprovacao, AprovarWorkflowAlteracaoNotaConselhoCommand request)
        {
            await AlterarNota(notaEmAprovacao);
            await ExcluirWorkFlow(notaEmAprovacao);
            await GerarParecerConclusivo(notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno, notaEmAprovacao.UsuarioSolicitanteId);
        }

        private async Task ExcluirWorkFlow(WFAprovacaoNotaConselho notaEmAprovacao)
        {
            await mediator.Send(new ExcluirWfAprovacaoNotaConselhoClasseCommand(notaEmAprovacao.Id));
        }

        private async Task AlterarNota(WFAprovacaoNotaConselho notaEmAprovacao)
        {
            var notaConselhoClasse = notaEmAprovacao.ConselhoClasseNota.Clone();

            notaConselhoClasse.Nota = notaEmAprovacao.Nota;

            notaConselhoClasse.ConceitoId = notaEmAprovacao.ConceitoId;

            await mediator.Send(new SalvarConselhoClasseNotaCommand(notaConselhoClasse));

            var periodoEscolar = notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.PeriodoEscolar;

            var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.CodigoTurma, notaConselhoClasse.ConselhoClasseAluno.AlunoCodigo, consideraInativos: true));

            if (aluno == null)
                throw new NegocioException($"Não foram encontrados alunos para a turma {notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.CodigoTurma} no Eol");

            var consolidacaoNotaAlunoDto = new ConsolidacaoNotaAlunoDto()
            {
                AlunoCodigo = notaConselhoClasse.ConselhoClasseAluno.AlunoCodigo,
                TurmaId = notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.Id,
                Bimestre = periodoEscolar == null ? (int?)null : periodoEscolar.Bimestre,
                AnoLetivo = notaEmAprovacao.ConselhoClasseNota.ConselhoClasseAluno.ConselhoClasse.FechamentoTurma.Turma.AnoLetivo,
                Nota = notaConselhoClasse.Nota,
                ConceitoId = notaConselhoClasse.ConceitoId,
                ComponenteCurricularId = notaConselhoClasse.ComponenteCurricularCodigo,
                Inativo = aluno.Inativo,
                ConselhoClasse = true
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
