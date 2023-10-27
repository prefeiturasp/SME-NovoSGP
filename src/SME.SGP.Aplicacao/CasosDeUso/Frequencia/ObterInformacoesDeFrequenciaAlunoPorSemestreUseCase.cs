using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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

        public async Task<IEnumerable<FrequenciaAlunoBimestreDto>> 
            
            Executar(FiltroTurmaAlunoSemestreDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException("A Turma informada não foi encontrada");

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(dto.AlunoCodigo.ToString(),
                turma.AnoLetivo));
            if (aluno.EhNulo())
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

            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Não foi possível encontrar o período escolar da turma.");

            var bimestres = new List<FrequenciaAlunoBimestreDto>();

            if(turma.ModalidadeCodigo == Modalidade.EducacaoInfantil)
            {
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

                if(dados1.NaoEhNulo() || dados2.NaoEhNulo())
                    bimestres = TratarMediaBimestresParaSemestreInfantil(dados1, dados2, semestre == 1 ? 1 : 3);
        
            }
            else
            {
                if (semestre == 1)
                {
                    bimestres.Add(await ObterInformacoesBimestre(turma, alunoCodigo, tipoCalendarioId,
                        periodosEscolares.First(a => a.Bimestre == 1), componenteCurricularId));
                    bimestres.Add(await ObterInformacoesBimestre(turma, alunoCodigo, tipoCalendarioId,
                        periodosEscolares.First(a => a.Bimestre == 2), componenteCurricularId));

                }
                else
                {
                    bimestres.Add(await ObterInformacoesBimestre(turma, alunoCodigo, tipoCalendarioId,
                        periodosEscolares.First(a => a.Bimestre == 3), componenteCurricularId));
                    bimestres.Add(await ObterInformacoesBimestre(turma, alunoCodigo, tipoCalendarioId,
                        periodosEscolares.First(a => a.Bimestre == 4), componenteCurricularId));
                }
            }
            
            return bimestres.Where(bimestre => bimestre.NaoEhNulo());
        }

        public List<FrequenciaAlunoBimestreDto> TratarMediaBimestresParaSemestreInfantil(FrequenciaAlunoBimestreDto dados1, FrequenciaAlunoBimestreDto dados2, int bimestreReferencia)
        {
            int somatorioAusencias = 0;
            int somatorioAulasRealizadas = 0;
            int somatorioCompensacoes = 0;
            double? mediaFrequencia;
            var bimestres = new List<FrequenciaAlunoBimestreDto>();

            somatorioAulasRealizadas = (dados1.EhNulo() ? 0 : dados1.AulasRealizadas) + (dados2.EhNulo() ? 0 : dados2.AulasRealizadas);
            somatorioAusencias = (dados1.EhNulo() ? 0 : dados1.Ausencias) + (dados2.EhNulo() ? 0 : dados2.Ausencias);
            somatorioCompensacoes = (dados1.EhNulo() ? 0 : dados1.Compensacoes) + (dados2.EhNulo() ? 0 : dados2.Compensacoes);
            mediaFrequencia = (dados1 ?? dados2).CalcularPercentualFrequencia(somatorioAulasRealizadas, somatorioAusencias - somatorioCompensacoes);

            mediaFrequencia = mediaFrequencia.NaoEhNulo() ? Math.Round(mediaFrequencia.Value, 2) : mediaFrequencia;

            bimestres.Add(new FrequenciaAlunoBimestreDto
            {
                Frequencia = mediaFrequencia,
                Ausencias = somatorioAusencias,
                AulasRealizadas = somatorioAulasRealizadas,
                Semestre = bimestreReferencia <= 2 ? 1 : 2,
                Bimestre = "0"
            });

            return bimestres;
        }

        private async Task<FrequenciaAlunoBimestreDto> ObterInformacoesBimestre(Turma turma, string alunoCodigo,
            long tipoCalendarioId, PeriodoEscolar periodoEscolar, long componenteCurricularId)
        {
            var frequenciaAlunoBimestre = new FrequenciaAlunoBimestreDto();

            frequenciaAlunoBimestre.Bimestre = periodoEscolar.Bimestre.ToString();

            var frequenciasRegistradas = await mediator.Send(new ObterFrequenciaBimestresQuery(alunoCodigo,
                periodoEscolar.Bimestre, turma.CodigoTurma, TipoFrequenciaAluno.Geral));
            
            if (frequenciasRegistradas.NaoEhNulo() && frequenciasRegistradas.Any())
            {
                var frequencia = frequenciasRegistradas.FirstOrDefault();
                frequenciaAlunoBimestre.Ausencias = frequencia.QuantidadeAusencias;
                frequenciaAlunoBimestre.Frequencia = (frequencia?.Frequencia).NaoEhNulo() ? frequencia.Frequencia : 0;
                frequenciaAlunoBimestre.AulasRealizadas = frequencia.TotalAulas;
            }
            else
            {
                var alunoPossuiFrequenciaRegistrada = await mediator.Send(
                    new ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery(alunoCodigo,
                        componenteCurricularId.ToString(), turma.CodigoTurma, new[] {periodoEscolar.Bimestre}));
                if (alunoPossuiFrequenciaRegistrada.EhNulo() || !alunoPossuiFrequenciaRegistrada.Any())
                {
                    return null;
                }

                frequenciaAlunoBimestre.AulasRealizadas =
                    await mediator.Send(new ObterAulasDadasPorTurmaIdEPeriodoEscolarQuery(turma.Id,
                        new List<long> {periodoEscolar.Id}, tipoCalendarioId));
                frequenciaAlunoBimestre.Ausencias = 0;
                frequenciaAlunoBimestre.Frequencia = alunoPossuiFrequenciaRegistrada.FirstOrDefault().PercentualFrequencia;
            }

            return frequenciaAlunoBimestre;
        }
    }
}