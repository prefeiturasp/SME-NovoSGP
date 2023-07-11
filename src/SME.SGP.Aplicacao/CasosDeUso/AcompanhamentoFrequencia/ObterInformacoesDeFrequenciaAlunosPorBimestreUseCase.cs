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

            var ehBimestreFinal = BimestreFinal == dto.Bimestre;

            var alunos = (await mediator.Send(new ObterAlunosDentroPeriodoQuery(turma.CodigoTurma,
                    (bimestreDoPeriodo.PeriodoInicio, bimestreDoPeriodo.PeriodoFim), ehBimestreFinal)))
                .DistinctBy(a => a.CodigoAluno)
                .OrderBy(a => a.NomeSocialAluno ?? a.NomeAluno);

            if (!alunos?.Any() ?? true)
                throw new NegocioException("Os alunos da turma não foram encontrados.");

            return BimestreFinal == dto.Bimestre
                ? await ObterFrequenciaAlunosBimestreFinalAsync(turma, alunos, dto.ComponenteCurricularId, tipoCalendarioId)
                : await ObterFrequenciaAlunosBimestresRegularesAsync(turma, alunos, dto.ComponenteCurricularId, tipoCalendarioId, periodoAtual);
        }

        private async Task<FrequenciaAlunosPorBimestreDto> ObterFrequenciaAlunosBimestresRegularesAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos, long componenteCurricularId, long tipoCalendarioId, PeriodoEscolar periodoEscolar)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var codigosComponentesTerritorioCorrespondentes = await mediator
                .Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(componenteCurricularId, turma.CodigoTurma, usuarioLogado.EhProfessor() ? usuarioLogado.Login : null));

            var componentesCurricularesId = new List<long>() { componenteCurricularId };
            if (codigosComponentesTerritorioCorrespondentes != default)
                componentesCurricularesId.AddRange(codigosComponentesTerritorioCorrespondentes.Select(cc => long.Parse(cc.codigoComponente)).Except(componentesCurricularesId));

            var professorRf = codigosComponentesTerritorioCorrespondentes != default ? codigosComponentesTerritorioCorrespondentes.FirstOrDefault().professor : null;
            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, componentesCurricularesId.ToArray(), tipoCalendarioId, periodoEscolar.Bimestre, professorRf);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.CodigoTurma, componentesCurricularesId.ToArray(), tipoCalendarioId, periodoEscolar.Id, professorRf));

            var frequenciaAlunosComTotalizadores = await ObterFrequenciaAlunosRegistradaFinalAsync(turma, componentesCurricularesId.ToArray(), new List<long> { periodoEscolar.Id }, alunos, professorRf);
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

        private async Task<(long codigo, string rf)> ObterComponenteTerritorioSaberERfLogin(Turma turma, long componenteCurricularId)
        {
            var codigoComponenteTerritorioCorrespondente = ((long)0, (string)null);
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuarioLogado.EhProfessor())
            {
                var componentesProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turma.CodigoTurma, usuarioLogado.Login, usuarioLogado.PerfilAtual));

                var componenteCorrespondente = componentesProfessor.FirstOrDefault(cp => cp.Codigo.Equals(componenteCurricularId) || cp.CodigoComponenteTerritorioSaber.Equals(componenteCurricularId) || (cp.CodigoComponenteCurricularPai.HasValue && cp.CodigoComponenteCurricularPai.Value.Equals(componenteCurricularId)));

                if (componenteCorrespondente != null)
                    codigoComponenteTerritorioCorrespondente = (componenteCorrespondente.TerritorioSaber && componenteCorrespondente.Codigo.Equals(componenteCurricularId) ? componenteCorrespondente.CodigoComponenteTerritorioSaber : componenteCorrespondente.Codigo, usuarioLogado.CodigoRf);

                return codigoComponenteTerritorioCorrespondente;
            }

            if (usuarioLogado.EhProfessorCj())
            {
                var professores = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(turma.Id));

                var professor = professores.FirstOrDefault(p => p.DisciplinasId.Contains(componenteCurricularId));
                if (professor != null && !String.IsNullOrEmpty(professor.ProfessorRf))
                {
                    var componentesProfessor = await mediator.Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turma.CodigoTurma, professor.ProfessorRf, Perfis.PERFIL_PROFESSOR));
                    var componenteProfessorRelacionado = componentesProfessor.FirstOrDefault(cp => cp.CodigoComponenteTerritorioSaber.Equals(componenteCurricularId));
                    if (componenteProfessorRelacionado != null)
                        codigoComponenteTerritorioCorrespondente = (componenteProfessorRelacionado.Codigo, professor.ProfessorRf);
                }
                return codigoComponenteTerritorioCorrespondente;
            }

            return codigoComponenteTerritorioCorrespondente;
        }

        private async Task<int> ObterAulasPrevistasAsync(Turma turma, long[] componentesCurricularesId, long tipoCalendarioId, int? bimestre = null, string professor = null)
            => turma.ModalidadeCodigo != Modalidade.EducacaoInfantil
                ? await mediator.Send(new ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery(turma.CodigoTurma, tipoCalendarioId, componentesCurricularesId, bimestre, professor))
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

                var marcador = periodoEscolar != null ? await mediator.Send(new ObterMarcadorFrequenciaAlunoQuery(aluno, periodoEscolar, turma.ModalidadeCodigo)) : null;

                var alunoPossuiPlanoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, turma.AnoLetivo));

                var totalRemotos = frequenciaAlunoRegistrada?.TotalRemotos ?? default;

                var totalPresencas = frequenciaAlunoRegistrada?.TotalPresencas ?? default;

                var totalAulas = frequenciaAlunoRegistrada?.TotalAulas ?? default;

                var percentualFrequencia = string.Empty;

                if (frequenciaAlunoRegistrada != null)
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
            return  await mediator.Send(new ObterAlunosAtivosTurmaProgramaPapEolQuery(anoLetivo, alunosCodigos));
        }
        private async Task<DisciplinaDto> ObterComponenteCurricularAsync(long componenteCurricularId, bool? possuiTerritorio = false, string codigoTurma = null)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new[] { componenteCurricularId }, possuiTerritorio, codigoTurma));
            return componentes.FirstOrDefault();
        }

        private async Task<FrequenciaAlunosPorBimestreDto> ObterFrequenciaAlunosBimestreFinalAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunos, long componenteCurricularId, long tipoCalendarioId)
        {
            var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

            if (!periodosEscolares?.Any() ?? true)
                throw new NegocioException("Não foi possível encontrar os períodos escolares.");

            var periodosEscolaresIds = periodosEscolares.Select(x => x.Id);

            var bimestres = periodosEscolares.Select(x => x.Bimestre);

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var codigosComponentesTerritorioCorrespondentes = await mediator.Send(new ObterCodigosComponentesCurricularesTerritorioSaberEquivalentesPorTurmaQuery(componenteCurricularId, turma.CodigoTurma, usuarioLogado.EhProfessor() ? usuarioLogado.Login : null));

            var componentesCurricularesId = new List<long>() { componenteCurricularId };
            if (codigosComponentesTerritorioCorrespondentes != default)
                componentesCurricularesId.AddRange(codigosComponentesTerritorioCorrespondentes.Select(cc => long.Parse(cc.codigoComponente)).Except(componentesCurricularesId));

            var professorRf = codigosComponentesTerritorioCorrespondentes != default ? codigosComponentesTerritorioCorrespondentes.FirstOrDefault().professor : null;
            var aulasPrevistas = await ObterAulasPrevistasAsync(turma, componentesCurricularesId.ToArray(), tipoCalendarioId, professor: professorRf);
            var aulasDadas = await mediator.Send(new ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolarQuery(turma.CodigoTurma, componentesCurricularesId.ToArray(), tipoCalendarioId, periodosEscolaresIds, professorRf));
            var frequenciaAlunosComTotalizadores = await ObterFrequenciaAlunosRegistradaFinalAsync(turma, componentesCurricularesId.ToArray(), periodosEscolaresIds, alunos);
            var frequenciaAlunos = await ObterListagemFrequenciaAluno(alunos, turma, frequenciaAlunosComTotalizadores, null, frequenciaAlunosComTotalizadores.Any());

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

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunosRegistradaFinalAsync(Turma turma, long[] componentesCurricularesId, IEnumerable<long> periodosEscolaresIds, IEnumerable<AlunoPorTurmaResposta> alunos, string professor = null)
        {
            var frequenciaAlunosRegistrada = await mediator
                .Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, componentesCurricularesId, periodosEscolaresIds, professor));

            var alunosPorTurma = alunos.ToList();

            return frequenciaAlunosRegistrada
                .GroupBy(x => x.CodigoAluno)
                .Select(x => ObterFrequenciaAluno(x, alunosPorTurma).Result)
                .ToList();
        }

        private async Task<FrequenciaAluno> ObterFrequenciaAluno(IGrouping<string, FrequenciaAluno> agrupamentoAluno, IEnumerable<AlunoPorTurmaResposta> alunos)
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

            return periodoEscolar == null ? periodosEscolares.Select(p => p.Bimestre).Max() : periodoEscolar.Bimestre;
        }
    }
}