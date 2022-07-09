using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciasPorAlunoUseCase : AbstractUseCase, IObterOcorrenciasPorAlunoUseCase
    {
        public ObterOcorrenciasPorAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<OcorrenciasPorAlunoDto>> Executar(FiltroTurmaAlunoSemestreDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma == null)
                throw new NegocioException("A Turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(dto.AlunoCodigo.ToString(), turma.AnoLetivo, turma.Historica));
            if (aluno == null)
                throw new NegocioException("O Aluno informado não foi encontrado");

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == default)
                throw new NegocioException("O tipo de calendário da turma não foi encontrado.");


            // Obter bimestres
            var periodo = await ObterPeriodoPorSemestreETipoCalendario(dto.Semestre, tipoCalendarioId);

            return await mediator.Send(new ObterOcorrenciasPorAlunoQuery(dto.TurmaId, dto.AlunoCodigo, periodo.DataInicio, periodo.DataFim));
        }

        private async Task<PeriodoOcorrenciaPorAlunoDto> ObterPeriodoPorSemestreETipoCalendario(int semestre, long tipoCalendarioId)
        {
            var periodo = new PeriodoOcorrenciaPorAlunoDto();
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (periodosEscolares == null)
                throw new NegocioException("Não foi possível encontrar o período escolar da turma.");

            if (semestre == 1)
            {
                periodo.DataInicio = periodosEscolares.First(a => a.Bimestre == 1).PeriodoInicio;
                periodo.DataFim = periodosEscolares.First(a => a.Bimestre == 2).PeriodoFim;
            }
            else
            {
                periodo.DataInicio = periodosEscolares.First(a => a.Bimestre == 3).PeriodoInicio;
                periodo.DataFim = periodosEscolares.First(a => a.Bimestre == 4).PeriodoFim;
            }
            return periodo;
        }




    }
}
