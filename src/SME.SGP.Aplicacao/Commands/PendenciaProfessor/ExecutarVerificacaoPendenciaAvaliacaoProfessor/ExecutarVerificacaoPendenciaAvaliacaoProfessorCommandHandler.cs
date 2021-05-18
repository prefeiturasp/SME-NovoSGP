using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAvaliacaoProfessorCommandHandler : IRequestHandler<ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEol;

        public ExecutarVerificacaoPendenciaAvaliacaoProfessorCommandHandler(IMediator mediator, IServicoEol servicoEol)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }


        public async Task<bool> Handle(ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand request, CancellationToken cancellationToken)
        {
            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesQuery());

            var periodosEncerrando = await mediator.Send(new ObterPeriodosFechamentoEscolasPorDataFinalQuery(DateTime.Now.Date.AddDays(request.DiasParaGeracaoDePendencia)));
            foreach (var periodoEncerrando in periodosEncerrando)
            {
                try
                {
                    if (periodoEncerrando.PeriodoEscolar.TipoCalendario.Modalidade == ModalidadeTipoCalendario.Infantil)
                        continue;

                    var turmas = await mediator.Send(new ObterTurmasPorUeModalidadesAnoQuery(periodoEncerrando.PeriodoFechamento.UeId.Value,
                                                                                             periodoEncerrando.PeriodoEscolar.TipoCalendario.Modalidade.ObterModalidadesTurma(),
                                                                                             periodoEncerrando.PeriodoEscolar.TipoCalendario.AnoLetivo));

                    var turmasComAvaliacao = await mediator.Send(new ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQuery(periodoEncerrando.PeriodoFechamento.UeId.Value,
                                                                                                                            periodoEncerrando.PeriodoEscolar.PeriodoInicio,
                                                                                                                            periodoEncerrando.PeriodoEscolar.PeriodoFim));


                    // Filtra turmas seriadas 1º ao 9º ano
                    foreach (var turma in turmas.Where(c => Enumerable.Range(1, 9).Select(a => a.ToString()).Contains(c.Ano)))
                    {
                        try
                        {
                            var professoresTurma = await servicoEol.ObterProfessoresTitularesDisciplinas(turma.CodigoTurma);

                            foreach (var professorComponenteTurma in professoresTurma)
                            {
                                if (string.IsNullOrEmpty(professorComponenteTurma.ProfessorRf))
                                    continue;

                                try
                                {
                                    var componenteCurricular = componentesCurriculares.FirstOrDefault(c => c.Codigo == professorComponenteTurma.DisciplinaId.ToString()
                                                                                               && c.LancaNota);

                                    if (componenteCurricular != null)
                                    {
                                        if (turmasComAvaliacao.Any(c => c.TurmaId == turma.Id && c.ComponenteCurricularId == professorComponenteTurma.DisciplinaId))
                                            continue;

                                        if (!await ExistePendenciaProfessor(turma, professorComponenteTurma, periodoEncerrando.PeriodoEscolar.Id))
                                            await IncluirPendenciaProfessor(turma, professorComponenteTurma.DisciplinaId, professorComponenteTurma.ProfessorRf, periodoEncerrando.PeriodoEscolar.Bimestre, componenteCurricular.Descricao, periodoEncerrando.PeriodoEscolar.Id);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SentrySdk.CaptureException(ex);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            SentrySdk.CaptureException(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }

            return true;
        }

        private async Task<bool> ExistePendenciaProfessor(Turma turma, ProfessorTitularDisciplinaEol professorComponente, long periodoEscolarId)
            => await mediator.Send(new ExistePendenciaProfessorPorTurmaEComponenteQuery(turma.Id,
                                                                                        professorComponente.DisciplinaId,
                                                                                        periodoEscolarId,
                                                                                        professorComponente.ProfessorRf,
                                                                                        TipoPendencia.AusenciaDeAvaliacaoProfessor));

        private async Task IncluirPendenciaProfessor(Turma turma, long componenteCurricularId, string professorRf, int bimestre, string componenteCurricularNome, long periodoEscolarId)
        {
            var escolaUe = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var titulo = $"Ausência de avaliação no {bimestre}º bimestre {escolaUe}";

            var descricao = $"<i>O componente curricular {componenteCurricularNome} não possui nenhuma avaliação cadastrada no {bimestre}º bimestre - {escolaUe}</i>";
            var instrucao = "Você precisa criar uma avaliação para esta turma e componente curricular.";

            await mediator.Send(new SalvarPendenciaAusenciaDeAvaliacaoProfessorCommand(turma.Id, componenteCurricularId, periodoEscolarId, professorRf, titulo, descricao, instrucao));
            
        }
    }
}
