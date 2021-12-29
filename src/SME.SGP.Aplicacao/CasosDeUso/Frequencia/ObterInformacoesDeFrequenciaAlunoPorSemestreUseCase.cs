using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesDeFrequenciaAlunoPorSemestreUseCase : AbstractUseCase,
        IObterInformacoesDeFrequenciaAlunoPorSemestreUseCase
    {
        public ObterInformacoesDeFrequenciaAlunoPorSemestreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<FrequenciaAlunoBimestreDto>> Executar(FiltroTurmaAlunoSemestreDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma == null)
                throw new NegocioException("A Turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(dto.AlunoCodigo.ToString(),
                turma.AnoLetivo));
            if (aluno == null)
                throw new NegocioException("O Aluno informado não foi encontrado");

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == default)
                throw new NegocioException("O tipo de calendário da turma não foi encontrado.");

            return await ObterFrequenciaAlunoBimestre(turma, dto.AlunoCodigo.ToString(), dto.Semestre, tipoCalendarioId,
                dto.ComponenteCurricularId);
        }

        private async Task<IEnumerable<FrequenciaAlunoBimestreDto>> ObterFrequenciaAlunoBimestre(Turma turma,
            string alunoCodigo, int semestre, long tipoCalendarioId, long componenteCurricularId)
        {
            var dados1 = new FrequenciaAlunoBimestreDto();
            var dados2 = new FrequenciaAlunoBimestreDto();

            var periodosEscolares =
                await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (periodosEscolares == null)
                throw new NegocioException("Não foi possível encontrar o período escolar da turma.");

            List<FrequenciaAlunoBimestreDto> bimestres = new List<FrequenciaAlunoBimestreDto>();

            if (semestre == 1)
            {
                dados1 = await ObterInformacoesBimestre(turma, alunoCodigo, tipoCalendarioId,
                    periodosEscolares.First(a => a.Bimestre == 1), componenteCurricularId);
                dados2 = await ObterInformacoesBimestre(turma, alunoCodigo, tipoCalendarioId,
                    periodosEscolares.First(a => a.Bimestre == 2), componenteCurricularId);               
            }
            else
            {
                dados1 = await ObterInformacoesBimestre(turma, alunoCodigo, tipoCalendarioId,
                    periodosEscolares.First(a => a.Bimestre == 3), componenteCurricularId);
                dados2 = await ObterInformacoesBimestre(turma, alunoCodigo, tipoCalendarioId,
                    periodosEscolares.First(a => a.Bimestre == 4), componenteCurricularId);
            }

            bimestres = TratarMediaBimestresParaSemestre(dados1, dados2);
            return bimestres.Where(bimestre => bimestre != null);
        }

        public List<FrequenciaAlunoBimestreDto> TratarMediaBimestresParaSemestre(FrequenciaAlunoBimestreDto dados1, FrequenciaAlunoBimestreDto dados2)
        {
            int somatorioAusencias = 0;
            int somatorioAulasRealizadas = 0;
            double? mediaFrequencia;
            List<FrequenciaAlunoBimestreDto> bimestres = new List<FrequenciaAlunoBimestreDto>();

            somatorioAulasRealizadas = (dados1 == null ? 0 : dados1.AulasRealizadas) + (dados2 == null ? 0 : dados2.AulasRealizadas);
            somatorioAusencias = (dados1 == null ? 0 : dados1.Ausencias) + (dados2 == null ? 0 : dados2.Ausencias);
            mediaFrequencia = ((dados1 == null ? 0 : dados1.Frequencia) + (dados2 == null ? 0 : dados2.Frequencia)) / 2;

            bimestres.Add(new FrequenciaAlunoBimestreDto
            {
                Frequencia = mediaFrequencia,
                Ausencias = somatorioAusencias,
                AulasRealizadas = somatorioAulasRealizadas
            });

            return bimestres;
        }

        private async Task<FrequenciaAlunoBimestreDto> ObterInformacoesBimestre(Turma turma, string alunoCodigo,
            long tipoCalendarioId, PeriodoEscolar periodoEscolar, long componenteCurricularId)
        {
            FrequenciaAlunoBimestreDto dto = new FrequenciaAlunoBimestreDto();

            var frequenciasRegistradas = await mediator.Send(new ObterFrequenciaBimestresQuery(alunoCodigo,
                periodoEscolar.Bimestre, turma.CodigoTurma, TipoFrequenciaAluno.Geral));
            
            if (frequenciasRegistradas != null && frequenciasRegistradas.Any())
            {
                var frequencia = frequenciasRegistradas.FirstOrDefault();
                dto.Ausencias = frequencia.QuantidadeAusencias;
                dto.Frequencia = frequencia?.Frequencia != null ? frequencia.Frequencia : 0;
                dto.AulasRealizadas = frequencia.TotalAulas;
            }
            else
            {
                
                var alunoPossuiFrequenciaRegistrada = await mediator.Send(
                    new ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery(alunoCodigo,
                        componenteCurricularId.ToString(), turma.CodigoTurma, new[] {periodoEscolar.Bimestre}));
                if (alunoPossuiFrequenciaRegistrada == null || !alunoPossuiFrequenciaRegistrada.Any())
                {
                    return null;
                }

                dto.AulasRealizadas =
                    await mediator.Send(new ObterAulasDadasPorTurmaIdEPeriodoEscolarQuery(turma.Id,
                        new List<long> {periodoEscolar.Id}, tipoCalendarioId));
                dto.Ausencias = 0;
                dto.Frequencia = alunoPossuiFrequenciaRegistrada.FirstOrDefault().PercentualFrequencia;
            }

            return dto;
        }
    }
}