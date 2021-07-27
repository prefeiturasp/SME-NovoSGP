using MediatR;
using SME.SGP.Dominio;
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

        public ObterInformacoesDeFrequenciaAlunosPorBimestreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<FrequenciaAlunosPorBimestreDto> Executar(ObterFrequenciaAlunosPorBimestreDto dto)
        {
            var componenteCurricular = await ObterComponenteCurricularAsync(dto.ComponenteCurricularId);
            if (componenteCurricular is null)
                throw new NegocioException("O componente curricular informado não foi encontrado.");

            if (!componenteCurricular.RegistraFrequencia)
                throw new NegocioException("Este componente curricular não possui controle de frequência.");

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(dto.TurmaId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            if (tipoCalendarioId == default)
                throw new NegocioException("O tipo de calendário da turma não foi encontrado.");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma));
            if (!alunos?.Any() ?? true)
                throw new NegocioException("Os alunos da turma não foram encontrados.");

            return BimestreFinal == dto.Bimestre
                ? await ObterFrequenciaAlunosBimestreFinalAsync(turma, alunos, dto.ComponenteCurricularId, tipoCalendarioId)
                : await ObterFrequenciaAlunosBimestresRegularesAsync(turma, alunos, dto.ComponenteCurricularId, tipoCalendarioId, dto.Bimestre);
        }

        private async Task<FrequenciaAlunosPorBimestreDto> ObterFrequenciaAlunosBimestresRegularesAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos, long componenteCurricularId, long tipoCalendarioId, int? bimestre)
        {
            var periodoEscolar = bimestre is null
                ? await mediator.Send(new ObterPeriodoEscolarAtualPorTurmaQuery(turma, DateTime.Now))
                : await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre.GetValueOrDefault()));

            if (periodoEscolar is null)
                throw new NegocioException("Não foi possível encontrar o período escolar da turma.");

            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, componenteCurricularId, tipoCalendarioId, periodoEscolar.Bimestre);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.Id, componenteCurricularId, tipoCalendarioId, periodoEscolar.Id));

            var frequenciaAlunosRegistrada = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, componenteCurricularId, periodoEscolar.Id));
            var frequenciaAlunos = await DefinirFrequenciaAlunoListagemAsync(alunos, turma, frequenciaAlunosRegistrada, periodoEscolar);

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

        private async Task<IEnumerable<AlunoFrequenciaDto>> DefinirFrequenciaAlunoListagemAsync(IEnumerable<AlunoPorTurmaResposta> alunos, Turma turma, IEnumerable<FrequenciaAluno> frequenciaAlunosRegistrada, PeriodoEscolar periodoEscolar)
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

                double percentualFrequenciaRetorno = CriaPercentualFrequenciaRetorno(turmaPossuiFrequenciasRegistradas, frequenciaAlunoRegistrada);

                novaListaAlunos.Add(new AlunoFrequenciaDto
                {
                    AlunoRf = long.Parse(aluno.CodigoAluno),
                    Ausencias = ausencias,
                    Compensacoes = compensacoes,
                    Frequencia = percentualFrequenciaRetorno < 0 ? string.Empty : percentualFrequenciaRetorno.ToString(),
                    MarcadorFrequencia = marcador,
                    Nome = aluno.NomeAluno,
                    NumeroChamada = aluno.NumeroAlunoChamada,
                    PossuiJustificativas = ausencias > 0,
                    EhAtendidoAEE = alunoPossuiPlanoAEE
                });
            }

            return novaListaAlunos;
        }

        private static double CriaPercentualFrequenciaRetorno(bool turmaPossuiFrequenciasRegistradas, FrequenciaAluno frequenciaAlunoRegistrada)
        {

            if (turmaPossuiFrequenciasRegistradas)
            {
                if (frequenciaAlunoRegistrada != null)
                    return frequenciaAlunoRegistrada.PercentualFrequencia;

                return 100;
            }
            return double.MinValue;
        }

        private async Task<DisciplinaDto> ObterComponenteCurricularAsync(long componenteCurricularId)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new[] { componenteCurricularId }));
            return componentes.FirstOrDefault();
        }

        private async Task<FrequenciaAlunosPorBimestreDto> ObterFrequenciaAlunosBimestreFinalAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos, long componenteCurricularId, long tipoCalendarioId)
        {
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            if (!periodosEscolares?.Any() ?? true)
                throw new NegocioException("Não foi possível encontrar os períodos escolares.");

            var periodosEscolaresIds = periodosEscolares.Select(x => x.Id);

            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, componenteCurricularId, tipoCalendarioId);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.Id, componenteCurricularId, tipoCalendarioId, periodosEscolaresIds));

            var frequenciaAlunosRegistrada = await ObterFrequenciaAlunosRegistradaFinalAsync(turma, componenteCurricularId, tipoCalendarioId, periodosEscolaresIds);
            var frequenciaAlunos = await DefinirFrequenciaAlunoListagemAsync(alunos, turma, frequenciaAlunosRegistrada, null);

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

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunosRegistradaFinalAsync(Turma turma, long componenteCurricularId, long tipoCalendarioId, IEnumerable<long> periodosEscolaresIds)
        {
            var frequenciaAlunosRegistrada = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, componenteCurricularId, periodosEscolaresIds));
            return frequenciaAlunosRegistrada
                .GroupBy(x => x.CodigoAluno)
                .Select(x => new FrequenciaAluno
                {
                    CodigoAluno = x.Key,
                    TotalAulas = x.Sum(y => y.TotalAulas),
                    TotalAusencias = x.Sum(y => y.TotalAusencias),
                    TotalCompensacoes = x.Sum(y => y.TotalCompensacoes),
                })
                .ToList();
        }
    }
}