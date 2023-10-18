using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAvaliacaoCPCommandHandler : IRequestHandler<ExecutarVerificacaoPendenciaAvaliacaoCPCommand, bool>
    {
        private readonly IMediator mediator;

        public ExecutarVerificacaoPendenciaAvaliacaoCPCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExecutarVerificacaoPendenciaAvaliacaoCPCommand request, CancellationToken cancellationToken)
        {
            var periodosEncerrando = await mediator.Send(new ObterPeriodosFechamentoEscolasPorDataFinalQuery(DateTime.Now.Date.AddDays(request.DiasParaGeracaoDePendencia)));
            foreach (var periodoEncerrando in periodosEncerrando.Where(w=> w.PeriodoEscolar.TipoCalendario.Modalidade.NaoEhEducacaoInfantil()))
            {
                try
                {
                    var turmasSemAvaliacao = await mediator.Send(new ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQuery(periodoEncerrando.PeriodoEscolar.TipoCalendarioId,
                                                                                                                 periodoEncerrando.PeriodoEscolar.PeriodoInicio,
                                                                                                                 periodoEncerrando.PeriodoEscolar.PeriodoFim));

                    if (turmasSemAvaliacao.NaoEhNulo() && turmasSemAvaliacao.Any())
                    {
                        var componentesCurriculares = await mediator.Send(ObterComponentesCurricularesQuery.Instance);
                        foreach (var turmaSemAvaliacao in turmasSemAvaliacao.GroupBy(a => (a.TurmaCodigo, a.TurmaId)))
                        {
                            try
                            {
                                await IncluirPendenciaCP(turmaSemAvaliacao, componentesCurriculares, periodoEncerrando);

                            }
                            catch (Exception ex)
                            {
                                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na geração de pendência de avaliação da Turma", LogNivel.Negocio, LogContexto.Avaliacao, $"Turma {turmaSemAvaliacao.Key.TurmaCodigo}: {ex.Message}"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na verificação da pendência de avaliação do CP.", LogNivel.Negocio, LogContexto.Avaliacao, ex.Message));
                }
            }

            return true;
        }

        private async Task IncluirPendenciaCP(IGrouping<(string TurmaCodigo, long TurmaId), TurmaEComponenteDto> turmaSemAvaliacao, IEnumerable<ComponenteCurricularDto> componentesCurriculares, PeriodoFechamentoBimestre periodoEncerrando)
        {
            var professoresTurma = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turmaSemAvaliacao.Key.TurmaCodigo));
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaSemAvaliacao.Key.TurmaId));

            var pendenciaId = await ObterPendenciaIdDaTurma(turmaSemAvaliacao.Key.TurmaId);
            var gerarPendenciasCP = new List<(long componenteCurricularId, string professorRf)>();

            var fechamentosDaTurma = await mediator.Send(new ObterFechamentoTurmaDisciplinaPorTurmaIdQuery(long.Parse(turma.CodigoTurma)));

            foreach (var componenteCurricularNaTurma in turmaSemAvaliacao)
            {
                var professorComponente = professoresTurma.FirstOrDefault(c => c.DisciplinasId.Contains(componenteCurricularNaTurma.ComponenteCurricularId));
                var componenteCurricular = componentesCurriculares.FirstOrDefault(c => c.Codigo == componenteCurricularNaTurma.ComponenteCurricularId.ToString());

                if (!fechamentosDaTurma.Any(a=> a.DisciplinaId == componenteCurricularNaTurma.ComponenteCurricularId && a.PeriodoEscolarId == periodoEncerrando.PeriodoEscolarId))
                {
                    if (professorComponente.NaoEhNulo() && !await ExistePendenciaProfessor(pendenciaId, turma.Id, componenteCurricular.Codigo, professorComponente.ProfessorRf, periodoEncerrando.PeriodoEscolar.Id))
                        gerarPendenciasCP.Add((long.Parse(componenteCurricular.Codigo), professorComponente.ProfessorRf));
                }
            }

            if (gerarPendenciasCP.Any())
                await GerarPendenciasCP(pendenciaId, gerarPendenciasCP, turma, periodoEncerrando.PeriodoEscolar);
        }

        private async Task GerarPendenciasCP(long pendenciaId, List<(long componenteCurricularId, string professorRf)> gerarPendenciasProfessor, Turma turma, PeriodoEscolar periodoEscolar)
        {
            if (pendenciaId == 0)
                pendenciaId = await IncluirPendenciaCP(turma, periodoEscolar.Bimestre);

            await mediator.Send(new SalvarPendenciaAusenciaDeAvaliacaoCPCommand(pendenciaId, turma.Id, periodoEscolar.Id, turma.UeId, gerarPendenciasProfessor));
        }

        private async Task<long> IncluirPendenciaCP(Turma turma, int bimestre)
        {
            var escolaUe = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao})";
            var titulo = $"Ausência de avaliação no {bimestre}º bimestre {escolaUe}";

            var descricao = $"<i>Os componentes curriculares abaixo não possuem nenhuma avaliação cadastrada no {bimestre}º bimestre {escolaUe}</i>";
            var instrucao = "Oriente os professores a cadastrarem as avaliações.";

            return await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.AusenciaDeAvaliacaoCP, turma.UeId, turma.Id, descricao, instrucao, titulo));
        }

        private async Task<bool> ExistePendenciaProfessor(long pendenciaId, long turmaId, string componenteCurricularId, string professorRf, long periodoEscolarId)
            => pendenciaId != 0 &&
            await mediator.Send(new ExistePendenciaProfessorPorTurmaEComponenteQuery(turmaId,
                                                                                     long.Parse(componenteCurricularId),
                                                                                     periodoEscolarId,
                                                                                     professorRf,
                                                                                     TipoPendencia.AusenciaDeAvaliacaoCP));

        private async Task<long> ObterPendenciaIdDaTurma(long turmaId)
            => await mediator.Send(new ObterPendenciaIdPorTurmaQuery(turmaId, TipoPendencia.AusenciaDeAvaliacaoCP));
    }
}
