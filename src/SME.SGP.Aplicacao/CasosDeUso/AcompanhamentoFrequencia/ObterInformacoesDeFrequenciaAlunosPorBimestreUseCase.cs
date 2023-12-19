using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
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

            var componenteCurricular = await ObterComponenteCurricularAsync(dto.ComponenteCurricularId, turma.CodigoTurma);
            if (componenteCurricular is null)
                throw new NegocioException("O componente curricular informado não foi encontrado.");

            if (!componenteCurricular.RegistraFrequencia)
                throw new NegocioException("Este componente curricular não possui controle de frequência.");

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == default)
                throw new NegocioException("O tipo de calendário da turma não foi encontrado.");

            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendarioId));
            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Não foi encontrado período Escolar para a modalidade informada.");

            var bimestreAtual = dto.Bimestre;
            if (!bimestreAtual.HasValue || dto.Bimestre == 0)
                bimestreAtual = ObterBimestreAtual(periodosEscolares);

            var periodoAtual = periodosEscolares.FirstOrDefault(x => x.Bimestre == bimestreAtual);
            if (periodoAtual.EhNulo())
                throw new NegocioException("Não foi encontrado período escolar para o bimestre solicitado.");

            var bimestreDoPeriodo = await consultasPeriodoEscolar.ObterPeriodoEscolarPorData(tipoCalendarioId, periodoAtual.PeriodoFim);

            var ehBimestreFinal = BimestreFinal == dto.Bimestre;
            var periodoInicio = bimestreDoPeriodo.PeriodoInicio;
            var periodoFim = bimestreDoPeriodo.PeriodoFim;

            if (ehBimestreFinal)
            {
                var periodoFinal = MontaPeriodoEscolarFinalParaMarcador(periodosEscolares, turma.ModalidadeCodigo);

                if (periodoFinal != null)
                {
                    periodoInicio = periodoFinal.PeriodoInicio;
                    periodoFim = periodoFinal.PeriodoFim;
                }
            }

            var alunos = (await mediator.Send(new ObterAlunosDentroPeriodoQuery(turma.CodigoTurma,
                    (periodoInicio, periodoFim))))
                .DistinctBy(a => a.CodigoAluno)
                .OrderBy(a => a.NomeSocialAluno ?? a.NomeAluno);

            if (!alunos?.Any() ?? true)
                throw new NegocioException("Os alunos da turma não foram encontrados.");

            long[] componentes = new long[] { dto.ComponenteCurricularId };

            if (componenteCurricular.TerritorioSaber)
                componentes = componentes.Concat(new long[] { componenteCurricular.CodigoComponenteCurricularTerritorioSaber }).ToArray();

            return BimestreFinal == dto.Bimestre
                ? await ObterFrequenciaAlunosBimestreFinalAsync(turma, alunos, componentes, tipoCalendarioId)
                : await ObterFrequenciaAlunosBimestresRegularesAsync(turma, alunos, componentes, tipoCalendarioId, periodoAtual);
        }

        private async Task<FrequenciaAlunosPorBimestreDto> ObterFrequenciaAlunosBimestresRegularesAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos, long[] componentesCurricularesId, long tipoCalendarioId, PeriodoEscolar periodoEscolar)
        {
            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, componentesCurricularesId, tipoCalendarioId, periodoEscolar.Bimestre);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.CodigoTurma, componentesCurricularesId, tipoCalendarioId, periodoEscolar.Id));

            var frequenciaAlunosComTotalizadores = await ObterFrequenciaAlunosRegistradaFinalAsync(turma, componentesCurricularesId, new List<long> { periodoEscolar.Id }, alunos);
            var frequenciaAlunos = await ObterListagemFrequenciaAluno(alunos, turma, frequenciaAlunosComTotalizadores, periodoEscolar, frequenciaAlunosComTotalizadores.Any());

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

        private async Task<int> ObterAulasPrevistasAsync(Turma turma, long[] componentesCurricularesId, long tipoCalendarioId, int? bimestre = null)
            => turma.ModalidadeCodigo != Modalidade.EducacaoInfantil
                ? await mediator.Send(new ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery(turma.CodigoTurma, tipoCalendarioId, componentesCurricularesId, bimestre))
                : default;

        private async Task<IEnumerable<AlunoFrequenciaDto>> ObterListagemFrequenciaAluno(IEnumerable<AlunoPorTurmaResposta> alunos, Turma turma, IEnumerable<FrequenciaAluno> frequenciaAlunosComTotalizadores, PeriodoEscolar periodoEscolar, bool turmaPossuiFrequenciaRegistrada = false)
        {
            var novaListaAlunos = new List<AlunoFrequenciaDto>();
            var matriculadosTurmaPAP = await BuscarAlunosTurmaPAP(alunos.Select(x => x.CodigoAluno).ToArray(), turma.AnoLetivo);
            foreach (var aluno in alunos)
            {
                var frequenciaAlunoRegistrada = frequenciaAlunosComTotalizadores.FirstOrDefault(y => y.CodigoAluno == aluno.CodigoAluno);

                var totalAusencias = frequenciaAlunoRegistrada?.TotalAusencias ?? default;

                var totalCompensacoes = frequenciaAlunoRegistrada?.TotalCompensacoes ?? default;

                var marcador = periodoEscolar.NaoEhNulo() ? await mediator.Send(new ObterMarcadorFrequenciaAlunoQuery(aluno, periodoEscolar, turma.ModalidadeCodigo)) : null;

                var alunoPossuiPlanoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));

                var totalRemotos = frequenciaAlunoRegistrada?.TotalRemotos ?? default;

                var totalPresencas = frequenciaAlunoRegistrada?.TotalPresencas ?? default;

                var totalAulas = frequenciaAlunoRegistrada?.TotalAulas ?? default;

                var percentualFrequencia = string.Empty;

                if (frequenciaAlunoRegistrada.NaoEhNulo())
                    percentualFrequencia = frequenciaAlunoRegistrada.PercentualFrequenciaFormatado;

                novaListaAlunos.Add(new AlunoFrequenciaDto
                {
                    AlunoRf = long.Parse(aluno.CodigoAluno),
                    Ausencias = totalAusencias,
                    Compensacoes = totalCompensacoes,
                    Frequencia = percentualFrequencia,
                    MarcadorFrequencia = marcador,
                    Nome = aluno.NomeValido(),
                    NumeroChamada = aluno.ObterNumeroAlunoChamada(),
                    PossuiJustificativas = totalAusencias > 0,
                    EhAtendidoAEE = alunoPossuiPlanoAEE,
                    EhMatriculadoTurmaPAP = matriculadosTurmaPAP.Any(x => x.CodigoAluno.ToString() == aluno.CodigoAluno),
                    Remotos = totalRemotos,
                    Presencas = totalPresencas,
                    TotalAulas = totalAulas
                });
            }

            return novaListaAlunos;
        }

        private async Task<IEnumerable<AlunosTurmaProgramaPapDto>> BuscarAlunosTurmaPAP(string[] alunosCodigos, int anoLetivo)
        {
            return await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        private async Task<DisciplinaDto> ObterComponenteCurricularAsync(long componenteCurricularId, string codigoTurma = null)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new[] { componenteCurricularId }, codigoTurma));
            return componentes.FirstOrDefault();
        }

        private async Task<FrequenciaAlunosPorBimestreDto> ObterFrequenciaAlunosBimestreFinalAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos, long[] componenteCurricularId, long tipoCalendarioId)
        {
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (!periodosEscolares?.Any() ?? true)
                throw new NegocioException("Não foi possível encontrar os períodos escolares.");

            var periodosEscolaresIds = periodosEscolares.Select(x => x.Id);
            var bimestres = periodosEscolares.Select(x => x.Bimestre);

            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, componenteCurricularId, tipoCalendarioId);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.CodigoTurma, componenteCurricularId, tipoCalendarioId, periodosEscolaresIds));
            var frequenciaAlunosComTotalizadores = await ObterFrequenciaAlunosRegistradaFinalAsync(turma, componenteCurricularId, periodosEscolaresIds, alunos);
            var periodoEscolar = MontaPeriodoEscolarFinalParaMarcador(periodosEscolares, turma.ModalidadeCodigo);
            var frequenciaAlunos = await ObterListagemFrequenciaAluno(alunos, turma, frequenciaAlunosComTotalizadores, periodoEscolar, frequenciaAlunosComTotalizadores.Any());

            return new FrequenciaAlunosPorBimestreDto
            {
                AulasDadas = aulasDadas,
                AulasPrevistas = aulasPrevistas,
                Bimestre = BimestreFinal,
                FrequenciaAlunos = frequenciaAlunos,
                MostraColunaCompensacaoAusencia = turma.ModalidadeCodigo != Modalidade.EducacaoInfantil,
                MostraLabelAulasPrevistas = turma.ModalidadeCodigo != Modalidade.EducacaoInfantil
            };
        }

        private PeriodoEscolar MontaPeriodoEscolarFinalParaMarcador(IEnumerable<PeriodoEscolar> periodos, Modalidade modalidadeTurma)
         => new PeriodoEscolar()
         {
             PeriodoInicio = periodos.FirstOrDefault(p => p.Bimestre == 1).PeriodoInicio,
             PeriodoFim = periodos.FirstOrDefault(p => modalidadeTurma == Modalidade.EJA ? p.Bimestre == 2 : p.Bimestre == 4).PeriodoFim
         };

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunosRegistradaFinalAsync(Turma turma, long[] componentesCurricularesId, IEnumerable<long> periodosEscolaresIds, IEnumerable<AlunoPorTurmaResposta> alunos, string professor = null)
        {
            var frequenciaAlunosRegistrada = await mediator
                .Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, componentesCurricularesId, periodosEscolaresIds));

            var alunosPorTurma = alunos.ToList();

            return frequenciaAlunosRegistrada
                .GroupBy(x => x.CodigoAluno)
                .Select(x => ObterFrequenciaAluno(x, alunosPorTurma))
                .ToList();
        }

        private FrequenciaAluno ObterFrequenciaAluno(IGrouping<string, FrequenciaAluno> agrupamentoAluno, IEnumerable<AlunoPorTurmaResposta> alunos)
        {
            var frequenciasAluno = agrupamentoAluno.ToList();

            var frequenciasConsideradas = from frequenciaAluno in frequenciasAluno
                                          join aluno in alunos on frequenciaAluno.CodigoAluno equals aluno.CodigoAluno
                                          where aluno.DataMatricula.Date <= frequenciaAluno.PeriodoFim.Date
                                          select frequenciaAluno;
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
            var dataPesquisa = DateTimeExtension.HorarioBrasilia();

            var periodoEscolar = periodosEscolares.FirstOrDefault(x => x.PeriodoInicio.Date <= dataPesquisa.Date && x.PeriodoFim.Date >= dataPesquisa.Date);

            return periodoEscolar.EhNulo() ? periodosEscolares.Select(p => p.Bimestre).Max() : periodoEscolar.Bimestre;
        }
    }
}