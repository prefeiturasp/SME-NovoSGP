using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAvaliacaoProfessorCommandHandler : IRequestHandler<ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutarVerificacaoPendenciaAvaliacaoProfessorCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Handle(ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand request, CancellationToken cancellationToken)
        {
            var componentesCurriculares = await mediator.Send(ObterComponentesCurricularesQuery.Instance);

            var periodosEncerrando = await mediator.Send(new ObterPeriodosFechamentoEscolasPorDataFinalQuery(DateTime.Now.Date.AddDays(request.DiasParaGeracaoDePendencia)));
            foreach (var periodoEncerrando in periodosEncerrando.Where(w=> w.PeriodoEscolar.TipoCalendario.Modalidade.NaoEhEducacaoInfantil()))
            {
                await ExecutaAcaoComTratamentoExcecao(async () =>
                {
                    var turmas = await mediator.Send(new ObterTurmasPorUeModalidadesAnoQuery(periodoEncerrando.PeriodoFechamento.UeId,
                                                                                             periodoEncerrando.PeriodoEscolar.TipoCalendario.Modalidade.ObterModalidades(),
                                                                                             periodoEncerrando.PeriodoEscolar.TipoCalendario.AnoLetivo));

                    var turmasComAvaliacao = await mediator.Send(new ObterQuantidadeAvaliacoesTurmaComponentePorUeNoPeriodoQuery(periodoEncerrando.PeriodoFechamento.UeId,
                                                                                                                            periodoEncerrando.PeriodoEscolar.PeriodoInicio,
                                                                                                                            periodoEncerrando.PeriodoEscolar.PeriodoFim));

                    // Filtra turmas seriadas 1º ao 9º ano
                    foreach (var turma in turmas.Where(c => Enumerable.Range(1, 9).Select(a => a.ToString()).Contains(c.Ano)))
                    {
                        await ExecutaAcaoComTratamentoExcecao(async () =>
                        {
                            var professoresTurma = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turma.CodigoTurma));
                            var fechamentosDaTurma = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdQuery(long.Parse(turma.CodigoTurma)));

                            foreach (var professorComponenteTurma in professoresTurma.Where(w => !string.IsNullOrEmpty(w.ProfessorRf)))
                            {
                                await ExecutaAcaoComTratamentoExcecao(async () =>
                                {
                                    var componenteCurricular = FiltrarComponenteCurricularProfLancanota(componentesCurriculares, professorComponenteTurma);
                                    if (await DeveIncluirPendenciaProfessor(turma, componenteCurricular,
                                                                  professorComponenteTurma, periodoEncerrando,
                                                                  turmasComAvaliacao, fechamentosDaTurma))
                                        await IncluirPendenciaProfessor(turma, professorComponenteTurma.DisciplinasId().First(), professorComponenteTurma.ProfessorRf, periodoEncerrando.PeriodoEscolar.Bimestre, componenteCurricular.Descricao, periodoEncerrando.PeriodoEscolar.Id);
                                }, "Erro na verificação da pendência de avaliação do Professor.");

                            }
                        },
                        "Erro na verificação da pendência de avaliação do Professor.");
                    }
                }, "Erro na verificação da pendência de avaliação do Professor.");
            }

            return true;
        }

        private async Task ExecutaAcaoComTratamentoExcecao(Func<Task> funcao, string msgErro)
        {
            try
            {
                await funcao();
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(msgErro, LogNivel.Negocio, LogContexto.Avaliacao, ex.Message));
            }
        }

        private ComponenteCurricularDto FiltrarComponenteCurricularProfLancanota(IEnumerable<ComponenteCurricularDto> componentesCurriculares,
                                                         ProfessorTitularDisciplinaEol professorComponenteTurma)
        => componentesCurriculares.FirstOrDefault(c => professorComponenteTurma.DisciplinasId().Contains(long.Parse(c.Codigo))
                                                               && c.LancaNota);

        private async Task<bool> DeveIncluirPendenciaProfessor(Turma turma, ComponenteCurricularDto componenteCurricular,
                                                               ProfessorTitularDisciplinaEol professorComponenteTurma,
                                                               PeriodoFechamentoBimestre periodoEncerrando,
                                                               IEnumerable<AvaliacoesPorTurmaComponenteDto> turmasComAvaliacao,
                                                               IEnumerable<TurmaFechamentoDisciplinaSituacaoDto> fechamentosDaTurma)
        {
            var componenteTurmaSemAvaliacao = !turmasComAvaliacao.Any(c => c.TurmaId == turma.Id && professorComponenteTurma.DisciplinasId().Contains(c.ComponenteCurricularId));
            var componenteTurmaSemFechamento = !fechamentosDaTurma.Any(a => professorComponenteTurma.DisciplinasId().Contains(a.DisciplinaId) && a.PeriodoEscolarId == periodoEncerrando.PeriodoEscolarId);

            return componenteCurricular.NaoEhNulo() &&
                   componenteTurmaSemAvaliacao &&
                   componenteTurmaSemFechamento &&
                   !(await ExistePendenciaProfessor(turma, professorComponenteTurma, periodoEncerrando.PeriodoEscolar.Id));
        }
        private async Task<bool> ExistePendenciaProfessor(Turma turma, ProfessorTitularDisciplinaEol professorComponente, long periodoEscolarId)
            => await mediator.Send(new ExistePendenciaProfessorPorTurmaEComponenteQuery(turma.Id,
                                                                                        professorComponente.DisciplinasId().First(),
                                                                                        periodoEscolarId,
                                                                                        professorComponente.ProfessorRf,
                                                                                        TipoPendencia.AusenciaDeAvaliacaoProfessor));

        private async Task IncluirPendenciaProfessor(Turma turma, long componenteCurricularId, string professorRf, int bimestre, string componenteCurricularNome, long periodoEscolarId)
        {
            var escolaUe = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var titulo = $"Ausência de avaliação no {bimestre}º bimestre {escolaUe}";

            var descricao = $"<i>O componente curricular {componenteCurricularNome} não possui nenhuma avaliação cadastrada no {bimestre}º bimestre - {escolaUe}</i>";
            var instrucao = "Você precisa criar uma avaliação para esta turma e componente curricular.";

            await mediator.Send(new SalvarPendenciaAusenciaDeAvaliacaoProfessorCommand(turma, componenteCurricularId, periodoEscolarId, professorRf, titulo, descricao, instrucao));

        }
    }
}
