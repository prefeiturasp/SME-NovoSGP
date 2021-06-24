using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase : IObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase
    {
        private readonly IMediator mediator;

        public ObterFrequenciasPorBimestresAlunoTurmaComponenteCurricularUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FrequenciaAluno>> Executar(FrequenciaPorBimestresAlunoTurmaComponenteCurricularDto dto)
        {
            var frequenciasAlunoRetorno = new List<FrequenciaAluno>();

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(dto.TurmaCodigo));

            if (turma == null)
                throw new NegocioException("Turma não encontrada!");

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));

            if (tipoCalendarioId <= 0)
                throw new NegocioException("Tipo calendário da turma não encontrada!");

            var frequenciasAluno = await mediator.Send(new ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery(
                                                                                 dto.AlunoCodigo,
                                                                                 dto.ComponenteCurricularId,
                                                                                 turma.CodigoTurma,
                                                                                 dto.Bimestres));
            var turmasCodigo = new string[] { turma.CodigoTurma };
            var componentesCurriculares = new string[] { dto.ComponenteCurricularId.ToString() };

            var aulasComponentesTurmas = await mediator.Send(new ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery(turmasCodigo, tipoCalendarioId, componentesCurriculares, dto.Bimestres));

            if (frequenciasAluno != null && frequenciasAluno.Any())
                frequenciasAlunoRetorno.AddRange(frequenciasAluno);

            foreach (var aulaComponenteTurma in aulasComponentesTurmas)
            {
                if (!frequenciasAlunoRetorno.Any(a => a.TurmaId == aulaComponenteTurma.TurmaCodigo && a.DisciplinaId == aulaComponenteTurma.ComponenteCurricularCodigo && a.Bimestre == aulaComponenteTurma.Bimestre))
                {
                    frequenciasAlunoRetorno.Add(new FrequenciaAluno()
                    {
                        CodigoAluno = dto.AlunoCodigo,
                        DisciplinaId = aulaComponenteTurma.ComponenteCurricularCodigo,
                        TurmaId = aulaComponenteTurma.TurmaCodigo,
                        TotalAulas = aulaComponenteTurma.AulasQuantidade,
                        Bimestre = aulaComponenteTurma.Bimestre
                    });
                }
            }

            return frequenciasAlunoRetorno;
        }
    }
}
