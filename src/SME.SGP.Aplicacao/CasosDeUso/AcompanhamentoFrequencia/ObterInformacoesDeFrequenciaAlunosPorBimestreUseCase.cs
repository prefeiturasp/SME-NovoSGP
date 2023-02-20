using MediatR;
using Microsoft.Diagnostics.Tracing.Parsers.AspNet;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesDeFrequenciaAlunosPorBimestreUseCase : AbstractUseCase, IObterInformacoesDeFrequenciaAlunosPorBimestreUseCase
    {
        private const int BimestreFinal = 0;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;

        public ObterInformacoesDeFrequenciaAlunosPorBimestreUseCase(IMediator mediator, IConsultasPeriodoEscolar consultasPeriodoEscolar) : base(mediator)
        {
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
        }

        public async Task<FrequenciaAlunosPorBimestreDto> Executar(ObterFrequenciaAlunosPorBimestreDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var componenteCurricular = await ObterComponenteCurricularAsync(dto.ComponenteCurricularId, dto.PossuiTerritorio, turma.CodigoTurma);
            if (componenteCurricular is null)
                throw new NegocioException("O componente curricular informado não foi encontrado.");

            if (!componenteCurricular.RegistraFrequencia)
                throw new NegocioException("Este componente curricular não possui controle de frequência.");            

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == default)
                throw new NegocioException("O tipo de calendário da turma não foi encontrado.");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId));
            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var bimestreAtual = dto.Bimestre;
            if (!bimestreAtual.HasValue || dto.Bimestre == 0)
                bimestreAtual = ObterBimestreAtual(periodosEscolares);

            var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestreAtual);
            if (periodoAtual == null)
                throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");

            var bimestreDoPeriodo = await consultasPeriodoEscolar.ObterPeriodoEscolarPorData(tipoCalendarioId, periodoAtual.PeriodoFim);

            var alunos = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.CodigoTurma, bimestreDoPeriodo.PeriodoFim));
            if (!alunos?.Any() ?? true)
                throw new NegocioException("Os alunos da turma não foram encontrados.");

            IEnumerable<AlunoPorTurmaResposta> alunosValidosComOrdenacao = null;

            if (turma.AnoLetivo == DateTime.Now.Year)
            {
                alunosValidosComOrdenacao = alunos.Where(a => a.DeveMostrarNaChamada(bimestreDoPeriodo.PeriodoFim, bimestreDoPeriodo.PeriodoInicio))
                                                  .OrderBy(a => a.NomeAluno)
                                                  .ThenBy(a => a.NomeValido());
            }
            else
            {
                alunosValidosComOrdenacao = alunos.Where(a => a.EstaAtivo(bimestreDoPeriodo.PeriodoInicio, bimestreDoPeriodo.PeriodoFim) || !a.SituacaoMatricula.Equals(SituacaoMatriculaAluno.VinculoIndevido) &&
                                                             (a.Inativo && a.DataSituacao >= bimestreDoPeriodo.PeriodoFim) && a.DataMatricula <= bimestreDoPeriodo.PeriodoFim)
                                                  .OrderBy(a => a.NomeAluno)
                                                  .ThenBy(a => a.NomeValido());
            }

            return BimestreFinal == dto.Bimestre
                ? await ObterFrequenciaAlunosBimestreFinalAsync(turma, alunosValidosComOrdenacao, dto.ComponenteCurricularId, tipoCalendarioId)
                : await ObterFrequenciaAlunosBimestresRegularesAsync(turma, alunosValidosComOrdenacao, dto.ComponenteCurricularId, tipoCalendarioId, dto.Bimestre, periodoAtual);
        }

        private async Task<FrequenciaAlunosPorBimestreDto> ObterFrequenciaAlunosBimestresRegularesAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos, long componenteCurricularId, long tipoCalendarioId, int? bimestre, PeriodoEscolar periodoEscolar)
        {
            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, componenteCurricularId, tipoCalendarioId, periodoEscolar.Bimestre);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.CodigoTurma, componenteCurricularId, tipoCalendarioId, periodoEscolar.Id));

            var frequenciaAlunosRegistrada = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, componenteCurricularId, periodoEscolar.Id));
            var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, componenteCurricularId.ToString(), periodoEscolar.Id));
            var frequenciaAlunos = await DefinirFrequenciaAlunoListagemAsync(alunos, turma, frequenciaAlunosRegistrada, periodoEscolar, turmaPossuiFrequenciaRegistrada);

            return new FrequenciaAlunosPorBimestreDto
            {
                AulasDadas = aulasDadas,
                AulasPrevistas = aulasPrevistas,
                Bimestre = periodoEscolar.Bimestre,
                FrequenciaAlunos = frequenciaAlunos.OrderBy(x => x.Nome),
                MostraColunaCompensacaoAusencia = turma.ModalidadeCodigo != Modalidade.EducacaoInfantil,
                MostraLabelAulasPrevistas = turma.ModalidadeCodigo != Modalidade.EducacaoInfantil
            };
        }

        private async Task<int> ObterAulasPrevistasAsync(Turma turma, long componenteCurricularId, long tipoCalendarioId, int? bimestre = null)
            => turma.ModalidadeCodigo != Modalidade.EducacaoInfantil
                ? await mediator.Send(new ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery(turma.CodigoTurma, tipoCalendarioId, componenteCurricularId, bimestre))
                : default;

        private async Task<IEnumerable<AlunoFrequenciaDto>> DefinirFrequenciaAlunoListagemAsync(IEnumerable<AlunoPorTurmaResposta> alunos, Turma turma, IEnumerable<FrequenciaAluno> frequenciaAlunosRegistrada, PeriodoEscolar periodoEscolar, bool turmaPossuiFrequenciaRegistrada = false)
        {
            var turmaPossuiFrequenciasRegistradas = frequenciaAlunosRegistrada.ToList().Count > 0;

            List<AlunoFrequenciaDto> novaListaAlunos = new List<AlunoFrequenciaDto>();
            foreach (var aluno in alunos)
            {
                var frequenciaAlunoRegistrada = frequenciaAlunosRegistrada.FirstOrDefault(y => y.CodigoAluno == aluno.CodigoAluno);
                var ausencias = frequenciaAlunoRegistrada?.TotalAusencias ?? default;
                var compensacoes = frequenciaAlunoRegistrada?.TotalCompensacoes ?? default;
                var marcador = periodoEscolar != null ? await mediator.Send(new ObterMarcadorFrequenciaAlunoQuery(aluno, periodoEscolar, turma.ModalidadeCodigo)) : null;
                var alunoPossuiPlanoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));
                var remotos = frequenciaAlunoRegistrada?.TotalRemotos ?? default;
                var presencas = frequenciaAlunoRegistrada?.TotalPresencas ?? default;
                var totalAulas = frequenciaAlunoRegistrada?.TotalAulas ?? default;

                var percentualFrequencia = frequenciaAlunoRegistrada == null && turmaPossuiFrequenciaRegistrada && totalAulas > 0
                ?
                "100"
                :
                frequenciaAlunoRegistrada != null
                ?
                frequenciaAlunoRegistrada?.PercentualFrequencia.ToString()
                :
                "";

                novaListaAlunos.Add(new AlunoFrequenciaDto
                {
                    AlunoRf = long.Parse(aluno.CodigoAluno),
                    Ausencias = ausencias,
                    Compensacoes = compensacoes,
                    Frequencia = percentualFrequencia,
                    MarcadorFrequencia = marcador,
                    Nome = aluno.NomeValido(),
                    NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                    PossuiJustificativas = ausencias > 0,
                    EhAtendidoAEE = alunoPossuiPlanoAEE,
                    Remotos = remotos,
                    Presencas = presencas,
                    TotalAulas = totalAulas
                });
            }

            return novaListaAlunos;
        }


        private async Task<DisciplinaDto> ObterComponenteCurricularAsync(long componenteCurricularId, bool? possuiTerritorio = false, string codigoTurma = null)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new[] { componenteCurricularId }, possuiTerritorio, codigoTurma));
            return componentes.FirstOrDefault();
        }

        private async Task<FrequenciaAlunosPorBimestreDto> ObterFrequenciaAlunosBimestreFinalAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos, long componenteCurricularId, long tipoCalendarioId)
        {
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            if (!periodosEscolares?.Any() ?? true)
                throw new NegocioException("Não foi possível encontrar os períodos escolares.");

            var periodosEscolaresIds = periodosEscolares.Select(x => x.Id);
            var bimestres = periodosEscolares.Select(x => x.Bimestre);

            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, componenteCurricularId, tipoCalendarioId);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.CodigoTurma, componenteCurricularId, tipoCalendarioId, periodosEscolaresIds));

            var frequenciaAlunosRegistrada = await ObterFrequenciaAlunosRegistradaFinalAsync(turma, componenteCurricularId, tipoCalendarioId, periodosEscolaresIds, bimestres);
            var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestresQuery(turma.CodigoTurma, componenteCurricularId.ToString(), periodosEscolaresIds.ToArray()));
            var frequenciaAlunos = await DefinirFrequenciaAlunoListagemAsync(alunos, turma, frequenciaAlunosRegistrada, null, turmaPossuiFrequenciaRegistrada);

            return new FrequenciaAlunosPorBimestreDto
            {
                AulasDadas = aulasDadas,
                AulasPrevistas = aulasPrevistas,
                Bimestre = BimestreFinal,
                FrequenciaAlunos = frequenciaAlunos.OrderBy(x => x.Nome),
                MostraColunaCompensacaoAusencia = turma.ModalidadeCodigo != Modalidade.EducacaoInfantil,
                MostraLabelAulasPrevistas = turma.ModalidadeCodigo != Modalidade.EducacaoInfantil
            };
        }

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunosRegistradaFinalAsync(Turma turma, long componenteCurricularId, long tipoCalendarioId, IEnumerable<long> periodosEscolaresIds, IEnumerable<int> bimestres)
        {
            var frequenciaAlunosRegistrada = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma,
                                                                                                    componenteCurricularId,
                                                                                                    periodosEscolaresIds));

            var aulasComponentesTurma = await mediator.Send(new ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery(new[] { turma.CodigoTurma },
                                                                                                                        tipoCalendarioId,
                                                                                                                        new string[] { componenteCurricularId.ToString() },
                                                                                                                        bimestres.ToArray()));


            return frequenciaAlunosRegistrada
                .GroupBy(x => x.CodigoAluno)
                .Select(x => ObterFrequenciaAluno(x, aulasComponentesTurma).Result)
                .ToList();
        }

        private async Task<FrequenciaAluno> ObterFrequenciaAluno(IGrouping<string, FrequenciaAluno> agrupamentoAluno, IEnumerable<TurmaComponenteQntAulasDto> aulasComponentesTurma)
        {
            var frequenciasAluno = agrupamentoAluno.ToList();

            var matriculasAluno = await mediator
                .Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(aulasComponentesTurma.First().TurmaCodigo), int.Parse(agrupamentoAluno.Key)));

            var frequenciasConsideradas = from f in frequenciasAluno
                                          from m in matriculasAluno
                                          where f.PeriodoFim.Date >= m.DataMatricula
                                          select f;
            return new FrequenciaAluno
            {
                CodigoAluno = agrupamentoAluno.Key,
                TotalAulas = frequenciasConsideradas.Sum(y => y.TotalAulas),
                TotalAusencias = frequenciasConsideradas.Sum(y => y.TotalAusencias),
                TotalCompensacoes = frequenciasConsideradas.Sum(y => y.TotalCompensacoes),
                TotalPresencas = frequenciasConsideradas.Sum(y => y.TotalPresencas),
                TotalRemotos = frequenciasConsideradas.Sum(y => y.TotalRemotos),
            };
        }

        private int ObterBimestreAtual(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var dataPesquisa = DateTime.Now;

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            if (periodoEscolar == null)
                return periodosEscolares.Select(p => p.Bimestre).Max();

            else return periodoEscolar.Bimestre;
        }
    }
}