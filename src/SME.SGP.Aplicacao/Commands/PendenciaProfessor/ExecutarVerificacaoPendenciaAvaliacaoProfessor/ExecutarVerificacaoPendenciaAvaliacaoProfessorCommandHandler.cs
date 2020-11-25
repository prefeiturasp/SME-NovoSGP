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
            var periodosEncerrando = await mediator.Send(new ObterPeriodosFechamentoEscolasPorDataFinalQuery(DateTime.Now.Date.AddDays(request.DiasParaGeracaoDePendencia)));
            foreach (var periodoEncerrando in periodosEncerrando)
            {
                var turmasSemAvaliacao = await mediator.Send(new ObterTurmaEComponenteSemAvaliacaoNoPeriodoPorUeQuery(periodoEncerrando.PeriodoFechamento.UeId.Value,
                                                                                                                        periodoEncerrando.PeriodoEscolar.TipoCalendarioId,
                                                                                                                        periodoEncerrando.PeriodoEscolar.PeriodoInicio,
                                                                                                                        periodoEncerrando.PeriodoEscolar.PeriodoFim));

                if (turmasSemAvaliacao != null && turmasSemAvaliacao.Any())
                {
                    var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesQuery());
                    foreach (var turmaSemAvaliacao in turmasSemAvaliacao.GroupBy(a => (a.TurmaCodigo, a.TurmaId)))
                    {
                        var professoresTurma = await servicoEol.ObterProfessoresTitularesDisciplinas(turmaSemAvaliacao.Key.TurmaCodigo);
                        var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaSemAvaliacao.Key.TurmaId));

                        foreach (var componenteCurricularNaTurma in turmaSemAvaliacao)
                        {
                            try
                            {
                                var professorComponente = professoresTurma.FirstOrDefault(c => c.DisciplinaId == componenteCurricularNaTurma.ComponenteCurricularId);
                                var componenteCurricular = componentesCurriculares.FirstOrDefault(c => c.Codigo == componenteCurricularNaTurma.ComponenteCurricularId.ToString());

                                if (professorComponente != null)
                                    if (!await ExistePendenciaProfessor(turma, componenteCurricularNaTurma, professorComponente, periodoEncerrando.PeriodoEscolar.Id))
                                        await IncluirPendenciaProfessor(turma, componenteCurricularNaTurma.ComponenteCurricularId, professorComponente.ProfessorRf, periodoEncerrando.PeriodoEscolar.Bimestre, componenteCurricular.Descricao, periodoEncerrando.PeriodoEscolar.Id);
                            }
                            catch (Exception ex)
                            {
                                SentrySdk.CaptureException(ex);
                            }
                        }
                    }
                }
            }

            return true;
        }

        private async Task<bool> ExistePendenciaProfessor(Turma turma, TurmaEComponenteDto componenteCurricularNaTurma, ProfessorTitularDisciplinaEol professorComponente, long periodoEscolarId)
            => await mediator.Send(new ExistePendenciaProfessorPorTurmaEComponenteQuery(turma.Id,
                                                                                        componenteCurricularNaTurma.ComponenteCurricularId,
                                                                                        periodoEscolarId,
                                                                                        professorComponente.ProfessorRf,
                                                                                        TipoPendencia.AusenciaDeAvaliacaoProfessor));

        private async Task IncluirPendenciaProfessor(Turma turma, long componenteCurricularId, string professorRf, int bimestre, string componenteCurricularNome, long periodoEscolarId)
        {
            var escolaUe = $"{turma.Ue.TipoEscola.ShortName()} {turma.Ue.Nome} ({turma.Ue.Dre.Abreviacao}) - Turma {turma.Nome}";
            var titulo = $"Ausência de avaliação no {bimestre}º bimestre {escolaUe}";

            var descricao = $"<i>O componente curricular {componenteCurricularNome} não possui nenhuma avaliação cadastrada no {bimestre}º bimestre - {escolaUe}</i>";
            var instrucao = "Acesse a tela de Calendário Escolar e confira os eventos da sua UE.";

            await mediator.Send(new SalvarPendenciaAusenciaDeAvaliacaoProfessorCommand(turma.Id, componenteCurricularId, periodoEscolarId, professorRf, titulo, descricao, instrucao));
            
        }
    }
}
