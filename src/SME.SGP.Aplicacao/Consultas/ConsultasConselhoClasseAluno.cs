﻿using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasseAluno : IConsultasConselhoClasseAluno
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private const int PRIMEIRO_BIMESTRE = 1;
        private const string PRIMEIRO_ANO_EM = "1";

        public ConsultasConselhoClasseAluno(IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno,
                                            IConsultasDisciplina consultasDisciplina,
                                            IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                            IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                            IServicoEol servicoEOL,
                                            IServicoUsuario servicoUsuario,
                                            IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo,
                                            IConsultasPeriodoFechamento consultasPeriodoFechamento,
                                            IMediator mediator)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> ExisteConselhoClasseUltimoBimestreAsync(Turma turma, string alunoCodigo)
        {
            var periodoEscolar = await ObterPeriodoUltimoBimestre(turma);

            var conselhoClasseUltimoBimestre = await repositorioConselhoClasseAluno.ObterPorPeriodoAsync(alunoCodigo, turma.Id, periodoEscolar.Id);

            if (conselhoClasseUltimoBimestre == null)
                return false;

            return await mediator.Send(new VerificaNotasTodosComponentesCurricularesQuery(alunoCodigo, turma, periodoEscolar.Bimestre, turma.Historica));
        }

        public async Task<IEnumerable<ConselhoDeClasseGrupoMatrizDto>> ObterListagemDeSinteses(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre)
        {
            var totalCompensacoesComponenteSemNota = await mediator.Send(new ObterTotalCompensacoesComponenteNaoLancaNotaQuery(codigoTurma, bimestre));
            totalCompensacoesComponenteSemNota = totalCompensacoesComponenteSemNota.Where(x => x.CodigoAluno == alunoCodigo);

            var totalAulasComponenteSemNota = Enumerable.Empty<TotalAulasNaoLancamNotaDto>();

            if (bimestre != (int)Bimestre.Final)
                totalAulasComponenteSemNota = await mediator.Send(new ObterTotalAulasNaoLancamNotaQuery(codigoTurma, bimestre, alunoCodigo));

            var retorno = new List<ConselhoDeClasseGrupoMatrizDto>();
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, turma.EhAnoAnterior()));

            if (fechamentoTurma != null)
                turma = fechamentoTurma?.Turma;
            else
            {
                if (!turma.EhAnoAnterior())
                    throw new NegocioException(MensagemNegocioFechamentoTurma.NAO_EXISTE_FECHAMENTO_TURMA);
            }

            if (turma.AnoLetivo != 2020 && turma.AnoLetivo == DateTime.Now.Year && bimestre == 0 && !(await ExisteConselhoClasseUltimoBimestreAsync(turma, alunoCodigo)))
                throw new NegocioException(MensagemNegocioConselhoClasse.ALUNO_NAO_POSSUI_CONSELHO_CLASSE_ULTIMO_BIMESTRE);

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurma(turma.CodigoTurma);
            if (disciplinas == null || !disciplinas.Any())
                return null;

            var gruposMatrizes = disciplinas.Where(x => !x.LancaNota && x.GrupoMatriz != null)
                                            .GroupBy(c => new { Id = c.GrupoMatriz?.Id, Nome = c.GrupoMatriz?.Nome });

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));
            var frequenciaAluno = await mediator.Send(new ObterFrequenciasAlunoComponentePorTurmasQuery(alunoCodigo, new string[] { turma.CodigoTurma }, tipoCalendarioId, bimestre));

            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma != null) turma = fechamentoTurma?.Turma;
            else
            {
                if (bimestre > 0)
                    periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
            }

            var registrosFrequencia = await mediator.Send(new ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery(alunoCodigo, new string[] { turma.CodigoTurma }, disciplinas.Select(d => !d.TerritorioSaber ? d.CodigoComponenteCurricular.ToString() : d.CodigoComponenteTerritorioSaber.ToString()).ToArray(),
                periodoEscolar?.Id));

            if (fechamentoTurma != null)
                turma = fechamentoTurma.Turma;

            foreach (var grupoDisiplinasMatriz in gruposMatrizes.OrderBy(k => k.Key.Nome))
            {
                var grupoMatriz = new ConselhoDeClasseGrupoMatrizDto()
                {
                    Id = grupoDisiplinasMatriz.Key.Id ?? 0,
                    Titulo = grupoDisiplinasMatriz.Key.Nome ?? "",
                    ComponenteSinteses = new List<ConselhoDeClasseComponenteSinteseDto>()
                };

                foreach (var componenteCurricular in grupoDisiplinasMatriz.Where(x => !x.LancaNota))
                {
                    if (componenteCurricular.TerritorioSaber)
                        componenteCurricular.Nome = disciplinas.First(d => d.CodigoComponenteCurricular == componenteCurricular.CodigoComponenteCurricular).Nome;

                    var componentePermiteFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(componenteCurricular.CodigoComponenteCurricular));

                    if (bimestre == (int)Bimestre.Final && componentePermiteFrequencia)
                    {
                        totalAulasComponenteSemNota = await mediator.Send(new ObterTotalAulasPorTurmaDisciplinaCodigoAlunoQuery(componenteCurricular.CodigoComponenteCurricular.ToString(), codigoTurma, alunoCodigo));
                    }
                    else if (bimestre == (int)Bimestre.Final && !componentePermiteFrequencia)
                    {
                        var totalAulasNaoPermitemFrequencia = await mediator.Send(
                            new ObterTotalAulasSemFrequenciaPorTurmaQuery(componenteCurricular.CodigoComponenteCurricular.ToString(), codigoTurma));

                        totalAulasComponenteSemNota = totalAulasNaoPermitemFrequencia.Select(x =>
                            new TotalAulasNaoLancamNotaDto
                            { DisciplinaId = Convert.ToInt32(x.DisciplinaId), TotalAulas = x.TotalAulas });
                    }

                    var componenteCurricularDto = await MapearDto(frequenciaAluno, componenteCurricular, bimestre, registrosFrequencia,
                        turma.ModalidadeCodigo, turma.AnoLetivo, totalAulasComponenteSemNota,
                        totalCompensacoesComponenteSemNota);

                    grupoMatriz.ComponenteSinteses.Add(componenteCurricularDto);
                }

                if (grupoMatriz.ComponenteSinteses.Any())
                    grupoMatriz.ComponenteSinteses = grupoMatriz.ComponenteSinteses.OrderBy(c => c.Nome).ToList();

                retorno.Add(grupoMatriz);
            }
            return retorno;
        }

        public async Task<ConselhoClasseAlunoNotasConceitosRetornoDto> ObterNotasFrequencia(long conselhoClasseId,
            long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre, bool consideraHistorico = false)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));

            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var anoLetivo = turma.AnoLetivo;
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, consideraHistorico));
            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma != null)
                turma = fechamentoTurma?.Turma;
            else if (bimestre > 0)
                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);

            if (tipoCalendario == null)
                throw new NegocioException(MensagemNegocioTipoCalendario.TIPO_CALENDARIO_NAO_ENCONTRADO);

            string[] turmasCodigosEOL = new string[1];
            string[] turmasCodigos;
            long[] conselhosClassesIds;

            var turmasItinerarioEnsinoMedio = (await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery())).ToList();
            var alunosEol = await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma, consideraInativos: true));
            var alunoNaTurma = alunosEol.FirstOrDefault(a => a.CodigoAluno == alunoCodigo);

            var codigosAlunos = alunosEol.Select(a => a.CodigoAluno).ToArray();
            var turmasComplementares = await mediator.Send(new ObterTurmasComplementaresPorAlunoQuery(codigosAlunos));
            var turmasComplementaresFiltradas = new TurmaComplementarDto();

            if (turmasComplementares != null && turmasComplementares.Any())
            {
                turmasComplementaresFiltradas = turmasComplementares.FirstOrDefault(t => t.TurmaRegularCodigo == turma.CodigoTurma && t.Semestre == turma.Semestre);
                turmasCodigosEOL = new string[] { turmasComplementaresFiltradas?.CodigoTurma ?? turma.CodigoTurma };
            }

            if ((turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma)) && !(bimestre == 0 && turma.EhEJA()))
            {
                var tiposTurmasParaConsulta = new List<int> { (int)turma.TipoTurma };

                tiposTurmasParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                tiposTurmasParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id));

                if (alunoNaTurma != null)
                    consideraHistorico = alunoNaTurma.Inativo;

                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo,
                    tiposTurmasParaConsulta, consideraHistorico, periodoEscolar?.PeriodoFim, semestre: turma.Semestre));

                if (!turmasCodigos.Any())
                {
                    turmasCodigos = new string[1] { turma.CodigoTurma };

                    if (!codigoTurma.Equals(turma.CodigoTurma))
                        turmasCodigos = new string[2] { codigoTurma, turma.CodigoTurma };
                }
                else if (!turmasCodigos.Contains(turma.CodigoTurma))
                    turmasCodigos = turmasCodigos.Concat(new[] { turma.CodigoTurma }).ToArray();

                conselhosClassesIds = await mediator
                    .Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos.Concat(turmasCodigosEOL).Distinct().ToArray(), periodoEscolar?.Id));

                if (conselhosClassesIds != null && !conselhosClassesIds.Any())
                    conselhosClassesIds = new long[1] { conselhoClasseId };
            }
            else
            {
                turmasCodigos = turmasCodigosEOL != null ? turmasCodigosEOL
                    .Append(turma.CodigoTurma).Distinct().ToArray() : new string[] { turma.CodigoTurma };

                var conselhosComplementares = await mediator
                    .Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigosEOL.ToArray(), periodoEscolar?.Id));

                if (conselhoClasseId > 0 && conselhosComplementares != null)
                {
                    conselhosClassesIds = conselhosComplementares != null ?
                        conselhosComplementares.Append(conselhoClasseId).Distinct().ToArray() : new long[] { conselhoClasseId };
                }
                else if (conselhoClasseId > 0)
                    conselhosClassesIds = new long[] { conselhoClasseId };
                else if (conselhosComplementares != null)
                    conselhosClassesIds = conselhosComplementares;
                else
                    conselhosClassesIds = null;
            }

            var periodosLetivos = (await mediator
                .Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendario.Id))).ToList();

            if (periodosLetivos == null || !periodosLetivos.Any())
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FORAM_ENCONTRADOS_PERIODOS_TIPO_CALENDARIO);

            var periodoInicio = periodoEscolar?.PeriodoInicio ?? periodosLetivos.OrderBy(pl => pl.Bimestre).First().PeriodoInicio;
            var periodoFim = periodoEscolar?.PeriodoFim ?? periodosLetivos.OrderBy(pl => pl.Bimestre).Last().PeriodoFim;

            var turmasComMatriculasValidas = await ObterTurmasComMatriculasValidas(alunoCodigo, turmasCodigos, periodoInicio, periodoFim);

            if (turmasComMatriculasValidas.Any())
                turmasCodigos = turmasComMatriculasValidas.ToArray();

            var notasConselhoClasseAluno = new List<NotaConceitoBimestreComponenteDto>();

            if (conselhosClassesIds != null)
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, alunoCodigo, bimestre));
                    notasConselhoClasseAluno.AddRange(notasParaAdicionar);
                }
            }

            if (turmasCodigos.Length > 0)
            {
                int quantidadeTurmas = turmasCodigos.Length;
                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos));

                if (turmas.Select(t => t.TipoTurma).Distinct().Count() == 1 && turma.ModalidadeCodigo != Modalidade.Medio)
                    turmasCodigos = new string[1] { turma.CodigoTurma };
                else if (ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(turmas, turma))
                    turmasCodigos = new string[1] { turma.CodigoTurma };

            }

            //Verificar as notas finais
            var notasFechamentoAluno = Enumerable.Empty<NotaConceitoBimestreComponenteDto>();
            var dadosAlunos = await mediator.Send(new ObterDadosAlunosQuery(codigoTurma, turma.AnoLetivo, null, true));
            var periodosEscolares = (await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id))).ToList();

            if (periodosEscolares != null)
            {
                var dataInicioPrimeiroBimestre = periodosEscolares.FirstOrDefault(pe => pe.Bimestre == PRIMEIRO_BIMESTRE).PeriodoInicio;
                dadosAlunos = dadosAlunos.Where(d => !d.EstaInativo() || d.EstaInativo() && d.SituacaoCodigo != SituacaoMatriculaAluno.VinculoIndevido && d.DataSituacao >= dataInicioPrimeiroBimestre);
            }

            var dadosAluno = dadosAlunos.FirstOrDefault(da => da.CodigoEOL.Contains(alunoCodigo));
            bool validaMatricula = false;

            if (turmasComMatriculasValidas.Contains(codigoTurma))
            {
                var turmasParaNotasFinais = turma.EhEJA() || await ValidaTurmaRegularJuntoAEdFisica(turmasCodigos.ToList(), codigoTurma)
                    ? turmasCodigos
                    : new string[] { codigoTurma };

                validaMatricula = !MatriculaIgualDataConclusaoAlunoTurma(alunoNaTurma);

                var turmasCodigosFiltro = turmasCodigos;

                turmasCodigosFiltro = turmasCodigosFiltro
                .Concat(turmasCodigosEOL).Distinct().ToArray();

                notasFechamentoAluno = fechamentoTurma != null && fechamentoTurma.PeriodoEscolarId.HasValue ?
                    await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigosFiltro, alunoCodigo, bimestre, dadosAluno.DataMatricula, !dadosAluno.EstaInativo() ? periodoFim : dadosAluno.DataSituacao, anoLetivo)) :
                    await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigosFiltro, alunoCodigo, dadosAluno.DataMatricula, !dadosAluno.EstaInativo() ? periodoFim : dadosAluno.DataSituacao, bimestre, validaMatricula));
            }

            var usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var disciplinasDaTurmaEol =
                (await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigos, usuarioAtual.PerfilAtual,
                    usuarioAtual.Login, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares, false))).ToList();

            if (turmasComplementaresFiltradas != null && turmasComplementaresFiltradas.TurmaRegularCodigo != null)
            {
                if (notasFechamentoAluno.Any(x => x.Nota != null && turma.EhTurmaEdFisica()))
                    ConverterNotaFechamentoAlunoNumerica(notasFechamentoAluno);

                var disciplinasDaTurmaTipo =
                (await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigosEOL, usuarioAtual.PerfilAtual,
                    usuarioAtual.Login, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares, false))).ToList();

                if (turmasComplementaresFiltradas.TipoTurma == TipoTurma.EdFisica)
                    disciplinasDaTurmaEol.AddRange(disciplinasDaTurmaTipo.Where(x => x.CodigoComponenteCurricular == 6));
            }

            var disciplinasCodigo = disciplinasDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray();
            var disciplinasDaTurma = (await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(disciplinasCodigo))).ToList();
            var areasDoConhecimento = (await mediator.Send(new ObterAreasConhecimentoQuery(disciplinasDaTurmaEol))).ToList();
            var ordenacaoGrupoArea = (await mediator.Send(new ObterOrdenacaoAreasConhecimentoQuery(disciplinasDaTurma, areasDoConhecimento))).ToList();
            var retorno = new ConselhoClasseAlunoNotasConceitosRetornoDto();
            var gruposMatrizesNotas = new List<ConselhoClasseAlunoNotasConceitosDto>();

            var frequenciasAluno = turmasComMatriculasValidas.Contains(codigoTurma) ?
                await ObterFrequenciaAlunoRefatorada(disciplinasDaTurmaEol, periodoEscolar, alunoNaTurma, tipoCalendario.Id, bimestre) :
                Enumerable.Empty<FrequenciaAluno>();

            var registrosFrequencia = (turmasComMatriculasValidas.Contains(codigoTurma) ?
                await mediator.Send(new ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery(alunoCodigo, turmasCodigos,
                    disciplinasCodigo.Select(d => d.ToString()).ToArray(), periodoEscolar?.Id)) :
                Enumerable.Empty<RegistroFrequenciaAlunoBimestreDto>()).ToList();

            var gruposMatrizes = disciplinasDaTurma.Where(c => c.GrupoMatrizNome != null && c.LancaNota).OrderBy(d => d.GrupoMatrizId).GroupBy(c => c.GrupoMatrizId).ToList();

            var permiteEdicao = (dadosAluno.EstaAtivo() && dadosAluno.DataMatricula.Date <= periodoFim) ||
                                    (dadosAluno.EstaInativo() && dadosAluno.DataSituacao.Date < periodoFim);

            var periodoMatricula = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(tipoCalendario.Id, alunoNaTurma.DataMatricula));

            foreach (var grupoDisiplinasMatriz in gruposMatrizes)
            {
                var conselhoClasseAlunoNotas = new ConselhoClasseAlunoNotasConceitosDto
                {
                    GrupoMatriz = disciplinasDaTurma.FirstOrDefault(dt => dt.GrupoMatrizId == grupoDisiplinasMatriz.Key)?.GrupoMatrizNome
                };

                var areasConhecimento = await mediator.Send(new MapearAreasConhecimentoQuery(grupoDisiplinasMatriz, areasDoConhecimento,
                    ordenacaoGrupoArea, Convert.ToInt64(grupoDisiplinasMatriz.Key)));

                foreach (var areaConhecimento in areasConhecimento)
                {
                    var componentes = await mediator.Send(new ObterComponentesAreasConhecimentoQuery(grupoDisiplinasMatriz, areaConhecimento));

                    foreach (var disciplina in componentes.Where(d => d.LancaNota).OrderBy(g => g.Nome).ToList())
                    {
                        var disciplinaEol = disciplinasDaTurmaEol.FirstOrDefault(d => d.CodigoComponenteCurricular == disciplina.Id);

                        var dataFim = periodoEscolar?.PeriodoFim ?? periodoFim;

                        var frequenciasAlunoParaTratar = frequenciasAluno.Where(a => a.DisciplinaId == disciplina.Id.ToString()
                                                         && (alunoNaTurma.DataMatricula <= dataFim
                                                         || MatriculaIgualDataConclusaoAlunoTurma(alunoNaTurma)));

                        frequenciasAlunoParaTratar = (periodoMatricula != null ? frequenciasAlunoParaTratar.Where(f => periodoMatricula.Bimestre <= f.Bimestre) : frequenciasAlunoParaTratar).ToList();

                        FrequenciaAluno frequenciaAluno;
                        var percentualFrequenciaPadrao = false;

                        if (frequenciasAlunoParaTratar == null || !frequenciasAlunoParaTratar.Any())
                            frequenciaAluno = new FrequenciaAluno() { DisciplinaId = disciplina.Id.ToString(), TurmaId = disciplinaEol.TurmaCodigo };
                        else if (frequenciasAlunoParaTratar.Count() == 1)
                            frequenciaAluno = frequenciasAlunoParaTratar.FirstOrDefault();
                        else
                        {
                            frequenciaAluno = new FrequenciaAluno
                            {
                                DisciplinaId = disciplina.CodigoComponenteCurricular.ToString(),
                                CodigoAluno = alunoCodigo,
                                TurmaId = turma.CodigoTurma,
                                TotalAulas = frequenciasAlunoParaTratar.Sum(a => a.TotalAulas),
                                TotalAusencias = frequenciasAlunoParaTratar.Sum(a => a.TotalAusencias),
                                TotalCompensacoes = frequenciasAlunoParaTratar.Sum(a => a.TotalCompensacoes)
                            };

                            percentualFrequenciaPadrao = true;

                            frequenciasAlunoParaTratar
                                .ToList()
                                .ForEach(f =>
                                {
                                    frequenciaAluno
                                        .AdicionarFrequenciaBimestre(f.Bimestre, tipoCalendario.AnoLetivo.Equals(2020) && f.TotalAulas.Equals(0) ? 100 : f.PercentualFrequencia);
                                });
                        }

                        if (disciplinaEol.Regencia)
                        {
                            conselhoClasseAlunoNotas.ComponenteRegencia = await ObterNotasFrequenciaRegencia(disciplina.CodigoComponenteCurricular,
                                frequenciaAluno, periodoEscolar, turma, notasConselhoClasseAluno, notasFechamentoAluno, disciplina.LancaNota,
                                permiteEdicao);
                        }
                        else
                        {
                            var turmaPossuiRegistroFrequencia = VerificarSePossuiRegistroFrequencia(alunoCodigo, disciplinaEol.TurmaCodigo,
                                disciplina.CodigoComponenteCurricular, periodoEscolar, frequenciasAlunoParaTratar, registrosFrequencia);

                            conselhoClasseAlunoNotas.ComponentesCurriculares.Add(await ObterNotasFrequenciaComponente(disciplina.Nome,
                                disciplina.CodigoComponenteCurricular, frequenciaAluno, periodoEscolar, turma, notasConselhoClasseAluno,
                                notasFechamentoAluno, turmaPossuiRegistroFrequencia, disciplina.LancaNota, percentualFrequenciaPadrao,
                                permiteEdicao, alunoCodigo, turmasComplementaresFiltradas));
                        }
                    }
                }

                if(conselhoClasseAlunoNotas.ComponentesCurriculares.Any())
                    conselhoClasseAlunoNotas.ComponentesCurriculares = conselhoClasseAlunoNotas.ComponentesCurriculares.OrderBy(c => c.Nome).ToList();
                
                if(conselhoClasseAlunoNotas.ComponenteRegencia != null)
                    conselhoClasseAlunoNotas.ComponenteRegencia.ComponentesCurriculares = conselhoClasseAlunoNotas.ComponenteRegencia.ComponentesCurriculares.OrderBy(c => c.Nome).ToList();

                gruposMatrizesNotas.Add(conselhoClasseAlunoNotas);
            }

            retorno.TemConselhoClasseAluno = conselhoClasseId > 0 && await VerificaSePossuiConselhoClasseAlunoAsync(conselhoClasseId, alunoCodigo);
            retorno.PodeEditarNota = permiteEdicao && await this.mediator.Send(new VerificaSePodeEditarNotaQuery(alunoCodigo, turma, periodoEscolar));
            retorno.NotasConceitos = gruposMatrizesNotas;

            return retorno;
        }

        private void ConverterNotaFechamentoAlunoNumerica(IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            foreach (var notaFechamento in notasFechamentoAluno)
            {
                if (notaFechamento.Nota != null)
                    if (notaFechamento.Nota >= 7)
                    {
                        notaFechamento.ConceitoId = 1;
                    }
                    else if (notaFechamento.Nota >= 5 && notaFechamento.Nota <= 7)
                    {
                        notaFechamento.ConceitoId = 2;
                    }
                    else
                        notaFechamento.ConceitoId = 3;
            }
        }

        private bool ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(IEnumerable<Turma> turmasAluno, Turma turmaFiltro)
            => turmasAluno.Select(t => t.TipoTurma).Distinct().Count() == 1 && turmaFiltro.ModalidadeCodigo == Modalidade.Medio && (turmaFiltro.AnoLetivo < 2021 || turmaFiltro.Ano == PRIMEIRO_ANO_EM);

        private async Task<bool> ValidaTurmaRegularJuntoAEdFisica(List<string> turmasCodigos, string turmaPrincipal)
        {
            if (turmasCodigos.Count() == 2)
            {
                turmasCodigos.Remove(turmaPrincipal);
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmasCodigos.FirstOrDefault()));
                if (turma.TipoTurma == Dominio.Enumerados.TipoTurma.EdFisica)
                    return true;
            }

            return false;
        }

        private bool MatriculaIgualDataConclusaoAlunoTurma(AlunoPorTurmaResposta alunoNaTurma)
        {
            return alunoNaTurma.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido && alunoNaTurma.DataMatricula.Date == alunoNaTurma.DataSituacao.Date;
        }

        public async Task<List<string>> ObterTurmasComMatriculasValidas(string alunoCodigo, string[] turmasCodigos, DateTime periodoInicio, DateTime periodoFim)
        {
            var turmasCodigosComMatriculasValidas = new List<string>();
            foreach (string codTurma in turmasCodigos)
            {
                var matriculasAluno = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(codTurma, alunoCodigo));
                if (matriculasAluno != null || matriculasAluno.Any())
                {
                    if ((matriculasAluno != null || matriculasAluno.Any()) && matriculasAluno.Any(m => m.PossuiSituacaoAtiva() || (!m.PossuiSituacaoAtiva() && m.DataSituacao >= periodoInicio && m.DataSituacao <= periodoFim) || (!m.PossuiSituacaoAtiva() && m.DataMatricula <= periodoFim && m.DataSituacao > periodoFim)))
                        turmasCodigosComMatriculasValidas.Add(codTurma);
                }
            }
            return turmasCodigosComMatriculasValidas;
        }

        private async Task<bool> VerificaSePossuiConselhoClasseAlunoAsync(long conselhoClasseId, string alunoCodigo)
        {
            var conselhoClasseAlunoId = await mediator.Send(new ObterConselhoClasseAlunoIdQuery(conselhoClasseId, alunoCodigo));
            return conselhoClasseAlunoId > 0;
        }

        private bool VerificarSePossuiRegistroFrequencia(string alunoCodigo, string turmaCodigo, long codigoComponenteCurricular, PeriodoEscolar periodoEscolar, IEnumerable<FrequenciaAluno> frequenciasAlunoParaTratar, IEnumerable<RegistroFrequenciaAlunoBimestreDto> registrosFrequencia)
        {
            return (frequenciasAlunoParaTratar != null && frequenciasAlunoParaTratar.Any()) ||
                    registrosFrequencia.Any(f => (periodoEscolar == null || f.Bimestre == periodoEscolar.Bimestre) &&
                                                f.CodigoAluno == alunoCodigo &&
                                                f.CodigoComponenteCurricular == codigoComponenteCurricular &&
                                                f.CodigoTurma == turmaCodigo);
        }

        private async Task<bool> VerificaSePodeEditarNota(string alunoCodigo, Turma turma, PeriodoEscolar periodoEscolar)
        {
            var turmaFechamento = await mediator.Send(new ObterAlunosAtivosPorTurmaCodigoQuery(turma.CodigoTurma, DateTimeExtension.HorarioBrasilia()));

            if (turmaFechamento == null || !turmaFechamento.Any())
                throw new NegocioException($"Não foi possível obter os dados da turma {turma.CodigoTurma}");

            var turmaFechamentoOrdenada = turmaFechamento.GroupBy(x => x.CodigoAluno).SelectMany(y => y.OrderByDescending(a => a.DataSituacao).Take(1));

            var aluno = turmaFechamentoOrdenada.Last(a => a.CodigoAluno == alunoCodigo);

            if (aluno == null)
                throw new NegocioException($"Não foi possível obter os dados do aluno {alunoCodigo}");

            var temPeriodoAberto = false;

            if (periodoEscolar != null)
            {
                temPeriodoAberto = await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(turma.CodigoTurma, aluno.DataSituacao, periodoEscolar.Bimestre);

                if (aluno.DataMatricula > periodoEscolar.PeriodoFim.Date) return false;
            }

            return aluno.PodeEditarNotaConceitoNoPeriodo(periodoEscolar, temPeriodoAberto);
        }

        public async Task<ParecerConclusivoDto> ObterParecerConclusivo(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, bool consideraHistorico = false)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.NAO_FOI_POSSIVEL_OBTER_DADOS_TURMA);

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, turma.AnoLetivo != DateTime.Now.Year));

            if (fechamentoTurma == null)
            {
                if (!turma.EhAnoAnterior())
                    throw new NegocioException(MensagemNegocioFechamentoTurma.NAO_EXISTE_FECHAMENTO_TURMA);
            }
            else
            {
                turma = fechamentoTurma.Turma;
            }

            if (turma.AnoLetivo != 2020 && !turma.EhAnoAnterior() && !await ExisteConselhoClasseUltimoBimestreAsync(turma, alunoCodigo))
                return new ParecerConclusivoDto() { EmAprovacao = false };

            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
            if (!turma.EhAnoAnterior() && (conselhoClasseAluno == null || !conselhoClasseAluno.ConselhoClasseParecerId.HasValue) && fechamentoTurma.PeriodoEscolarId == null)
                return await mediator.Send(new GerarParecerConclusivoPorConselhoFechamentoAlunoCommand(conselhoClasseId, fechamentoTurmaId, alunoCodigo));

            var parecerConclusivoDto = new ParecerConclusivoDto()
            {
                Id = conselhoClasseAluno?.ConselhoClasseParecerId != null ? conselhoClasseAluno.ConselhoClasseParecerId.Value : 0,
                Nome = conselhoClasseAluno?.ConselhoClasseParecer?.Nome,
                EmAprovacao = false
            };

            await VerificaEmAprovacaoParecerConclusivo(conselhoClasseAluno?.Id, parecerConclusivoDto);

            return parecerConclusivoDto;
        }

        private async Task VerificaEmAprovacaoParecerConclusivo(long? conselhoClasseAlunoId, ParecerConclusivoDto parecerConclusivoDto)
        {
            if (conselhoClasseAlunoId != null && conselhoClasseAlunoId > 0)
            {
                var wfAprovacaoParecerConclusivo = await mediator.Send(new ObterSePossuiParecerEmAprovacaoQuery(conselhoClasseAlunoId));

                if (wfAprovacaoParecerConclusivo != null)
                {
                    parecerConclusivoDto.Id = wfAprovacaoParecerConclusivo.ConselhoClasseParecerId.Value;
                    parecerConclusivoDto.Nome = wfAprovacaoParecerConclusivo.ConselhoClasseParecer.Nome;
                    parecerConclusivoDto.EmAprovacao = true;
                }
            }
        }

        public async Task<ConselhoClasseAluno> ObterPorConselhoClasseAsync(long conselhoClasseId, string alunoCodigo)
            => await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);

        private DisciplinaDto MapearDisciplinasDto(DisciplinaResposta componenteCurricular)
        {
            return new DisciplinaDto
            {
                CodigoComponenteCurricular = componenteCurricular.CodigoComponenteCurricular,
                Compartilhada = componenteCurricular.Compartilhada,
                LancaNota = componenteCurricular.LancaNota,
                Nome = componenteCurricular.Nome,
                Regencia = componenteCurricular.Regencia,
                RegistraFrequencia = componenteCurricular.RegistroFrequencia,
                TerritorioSaber = componenteCurricular.TerritorioSaber
            };
        }

        private ConselhoDeClasseComponenteSinteseDto MapearConselhoDeClasseComponenteSinteseDto(DisciplinaResposta componenteCurricular, FrequenciaAluno frequenciaDisciplina, string percentualFrequencia, SinteseDto parecerFinal, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas, IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto> totalCompensacoes, int bimestre)
        {
            var codigoComponenteCurricular = ObterCodigoComponenteCurricular(componenteCurricular);

            return new ConselhoDeClasseComponenteSinteseDto
            {
                Codigo = componenteCurricular.CodigoComponenteCurricular,
                Nome = componenteCurricular.Nome,
                TotalFaltas = frequenciaDisciplina?.TotalAusencias,
                PercentualFrequencia = ExibirPercentualFrequencia(percentualFrequencia, totalAulas, componenteCurricular.CodigoComponenteCurricular),
                ParecerFinal = parecerFinal?.Valor == null || !totalAulas.Any() ? string.Empty : parecerFinal?.Valor,
                ParecerFinalId = (int)(parecerFinal?.Id ?? default),
                TotalAulas = ExibirTotalAulas(totalAulas, componenteCurricular.CodigoComponenteCurricular),
                TotalAusenciasCompensadas = ExibirTotalCompensadas(totalCompensacoes, componenteCurricular.CodigoComponenteCurricular, bimestre)
            };
        }

        private string ExibirPercentualFrequencia(string percentualFrequencia, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas, long componenteCurricular)
        {
            var aulas = totalAulas.FirstOrDefault(x => x.DisciplinaId == componenteCurricular);

            if (aulas == null || String.IsNullOrEmpty(percentualFrequencia) || (percentualFrequencia == "0" && aulas == null))
                return "";

            return $"{percentualFrequencia}%";
        }

        private string ExibirTotalCompensadas(IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto> totalCompensacao, long codigoComponenteCurricular, int bimestre)
        {
            if (bimestre != (int)Bimestre.Final)
            {
                var compensacoes = totalCompensacao.FirstOrDefault(x => x.DisciplinaId == codigoComponenteCurricular);
                return compensacoes?.TotalCompensacoes ?? "0";
            }
            else
            {
                var compensacoes = totalCompensacao.Where(x => x.DisciplinaId == codigoComponenteCurricular);
                return compensacoes.Sum(x => Convert.ToInt64(x.TotalCompensacoes)).ToString();
            }

        }

        private string ExibirTotalAulas(IEnumerable<TotalAulasNaoLancamNotaDto> aulas, long codigoComponenteCurricular)
        {
            var totalAulas = aulas.FirstOrDefault(x => x.DisciplinaId == codigoComponenteCurricular);

            return totalAulas?.TotalAulas ?? "0";
        }

        private string ObterPercentualDeFrequencia(IEnumerable<FrequenciaAluno> frequenciaDisciplina)
        {
            var retorno = frequenciaDisciplina.Any() ? (frequenciaDisciplina.Sum(x => x.PercentualFrequencia) / frequenciaDisciplina.Count()).ToString() : "";
            return retorno;
        }

        private async Task<ConselhoDeClasseComponenteSinteseDto> MapearDto(IEnumerable<FrequenciaAluno> frequenciaAluno, DisciplinaResposta componenteCurricular, int bimestre, IEnumerable<RegistroFrequenciaAlunoBimestreDto> registrosFrequencia, Modalidade modalidade, int anoLetivo, IEnumerable<TotalAulasNaoLancamNotaDto> totalAulas, IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto> totalCompensacoes)
        {
            var dto = MapearDisciplinasDto(componenteCurricular);

            var codigoComponenteCurricular = ObterCodigoComponenteCurricular(componenteCurricular);

            var frequenciaComponente = frequenciaAluno.FirstOrDefault(a => a.DisciplinaId == codigoComponenteCurricular.ToString());
            var percentualFrequencia = CalcularPercentualFrequenciaComponente(frequenciaComponente, componenteCurricular, anoLetivo);

            var parecerFinal = bimestre == 0 && EhEjaCompartilhada(componenteCurricular, modalidade) == false
                                        ? await mediator.Send(new ObterSinteseAlunoQuery(String.IsNullOrEmpty(percentualFrequencia) ? 0 : double.Parse(percentualFrequencia), dto, anoLetivo))
                                        : null;

            var componenteSinteseAdicionar = MapearConselhoDeClasseComponenteSinteseDto(componenteCurricular,
                frequenciaComponente, percentualFrequencia, parecerFinal, totalAulas, totalCompensacoes, bimestre);

            return componenteSinteseAdicionar;
        }

        private string CalcularPercentualFrequenciaComponente(FrequenciaAluno frequenciaComponente, DisciplinaResposta componenteCurricular, int anoLetivo)
        {
            return anoLetivo == 2020 ?
                frequenciaComponente?.PercentualFrequenciaFinal.ToString() ?? "0" :
                frequenciaComponente?.PercentualFrequencia.ToString() ?? "0";
        }

        private bool EhEjaCompartilhada(DisciplinaResposta componenteCurricular, Modalidade modalidade)
        {
            const long componenteInformatica = 1061;
            const long componenteLeitura = 1060;

            return modalidade == Modalidade.EJA
                    && (componenteCurricular.CodigoComponenteCurricular == componenteInformatica || componenteCurricular.CodigoComponenteCurricular == componenteLeitura);
        }

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunoRefatorada(IEnumerable<DisciplinaDto> disciplinasDaTurma, PeriodoEscolar periodoEscolar, AlunoPorTurmaResposta alunoTurma,
            long tipoCalendarioId, int bimestre)
        {
            var frequenciasAlunoRetorno = new List<FrequenciaAluno>();
            var disciplinasId = disciplinasDaTurma.Select(a => a.CodigoComponenteCurricular.ToString()).Distinct().ToArray();
            var turmasCodigo = disciplinasDaTurma.Select(a => a.TurmaCodigo).Distinct().ToArray();

            int[] bimestres;
            if (periodoEscolar == null)
            {
                var periodosEscolaresTurma = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendarioId);

                if (periodosEscolaresTurma.Any())
                    bimestres = periodosEscolaresTurma.Select(a => a.Bimestre).ToArray();
                else throw new NegocioException(MensagemNegocioPeriodo.NAO_FORAM_ENCONTRADOS_PERIODOS_TURMA);
            }
            else bimestres = new int[] { bimestre };

            var frequenciasAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo
                .ObterPorAlunoTurmasDisciplinasDataAsync(alunoTurma.CodigoAluno,
                                                         TipoFrequenciaAluno.PorDisciplina,
                                                         disciplinasId,
                                                         turmasCodigo, bimestres);

            var aulasComponentesTurmas = await mediator
                .Send(new ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery(turmasCodigo, tipoCalendarioId, disciplinasId, bimestres, alunoTurma.DataMatricula, alunoTurma.Inativo ? alunoTurma.DataSituacao : null));

            if (frequenciasAluno != null && frequenciasAluno.Any())
                frequenciasAlunoRetorno.AddRange(frequenciasAluno);

            foreach (var aulaComponenteTurma in aulasComponentesTurmas)
            {
                if (!frequenciasAlunoRetorno.Any(a => a.TurmaId == aulaComponenteTurma.TurmaCodigo && a.DisciplinaId == aulaComponenteTurma.ComponenteCurricularCodigo && a.Bimestre == aulaComponenteTurma.Bimestre))
                {
                    frequenciasAlunoRetorno.Add(new FrequenciaAluno()
                    {
                        CodigoAluno = alunoTurma.CodigoAluno,
                        DisciplinaId = aulaComponenteTurma.ComponenteCurricularCodigo,
                        TurmaId = aulaComponenteTurma.TurmaCodigo,
                        TotalAulas = aulaComponenteTurma.AulasQuantidade,
                        Bimestre = aulaComponenteTurma.Bimestre,
                        PeriodoEscolarId = aulaComponenteTurma.PeriodoEscolarId
                    });
                }
            }

            return frequenciasAlunoRetorno;
        }

        private NotaBimestreDto ObterNotasFinaisFechamento(long codigoComponenteCurricular, int bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            double? notaConceito = null;
            // Busca nota do FechamentoNota
            var notaFechamento = notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == codigoComponenteCurricular);
            if (notaFechamento != null)
                notaConceito = notaFechamento.NotaConceito;

            return new NotaBimestreDto()
            {
                Bimestre = bimestre,
                NotaConceito = notaConceito
            };
        }

        private async Task<NotaPosConselhoDto> ObterNotasPosConselho(long componenteCurricularCodigo, int? bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno,
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota, bool visualizaNota, TurmaComplementarDto turmaComplementar = null)
        {
            // Busca nota do conselho de classe consultado
            var notaComponente = notasConselhoClasseAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo);
            var notaComponenteId = notaComponente?.ConselhoClasseNotaId;

            if (notaComponente == null || !notaComponente.NotaConceito.HasValue)
            {
                var notaComponenteFechamento = new NotaConceitoBimestreComponenteDto();

                if (turmaComplementar != null && turmaComplementar.EhEJA() && turmaComplementar.TurmaRegularCodigo != null)
                {
                    var codigosTurma = new string[] { turmaComplementar.CodigoTurma, turmaComplementar.TurmaRegularCodigo };

                    notaComponenteFechamento =
                        notasFechamentoAluno.FirstOrDefault(t => codigosTurma.Contains(t.TurmaCodigo) && t.ComponenteCurricularCodigo == componenteCurricularCodigo && t.Bimestre == bimestre && t.ConselhoClasseNotaId > 0)
                        ?? notasFechamentoAluno.FirstOrDefault(t => codigosTurma.Contains(t.TurmaCodigo) && t.ComponenteCurricularCodigo == componenteCurricularCodigo && t.Bimestre == bimestre);
                }
                else
                {
                    notaComponenteFechamento =
                        notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == bimestre && c.ConselhoClasseNotaId > 0)
                        ?? notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == bimestre);
                }
                notaComponente = notaComponenteFechamento;
            }

            var notaPosConselho = new NotaPosConselhoDto()
            {
                Id = visualizaNota ? notaComponenteId : null,
                Nota = visualizaNota ? notaComponente?.NotaConceito : null,
                PodeEditar = componenteLancaNota && visualizaNota
            };

            if (notaComponenteId.HasValue)
                await VerificaNotaEmAprovacao(notaComponenteId.Value, notaPosConselho);

            return notaPosConselho;
        }

        private async Task VerificaNotaEmAprovacao(long conselhoClasseNotaId, NotaPosConselhoDto nota)
        {
            double notaConselhoEmAprovacao = await mediator.Send(new ObterNotaConselhoEmAprovacaoQuery(conselhoClasseNotaId));

            if (notaConselhoEmAprovacao > 0)
            {
                nota.EmAprovacao = true;
                nota.Nota = notaConselhoEmAprovacao;
            }
            else
            {
                nota.EmAprovacao = false;
            }
        }

        private List<NotaBimestreDto> ObterNotasFechamentoOuConselho(long componenteCurricularCodigo, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var notasFinais = new List<NotaBimestreDto>();

            if (periodoEscolar != null)
                notasFinais.Add(ObterNotasFinaisFechamento(componenteCurricularCodigo, periodoEscolar.Bimestre, notasFechamentoAluno));
            else
                notasFinais.AddRange(ObterNotasFinaisConselho(componenteCurricularCodigo, notasFechamentoAluno));

            return notasFinais;
        }

        private IEnumerable<NotaBimestreDto> ObterNotasFinaisConselho(long codigoComponenteCurricular, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var notasPeriodos = new List<NotaBimestreDto>();

            var notasFechamentoBimestres = notasFechamentoAluno.Where(c => c.ComponenteCurricularCodigo == codigoComponenteCurricular && c.Bimestre.HasValue);
            foreach (var notaFechamento in notasFechamentoBimestres)
            {
                notasPeriodos.Add(new NotaBimestreDto()
                {
                    Bimestre = notaFechamento.Bimestre.Value,
                    NotaConceito = notaFechamento.NotaConceito
                });
            }

            return notasPeriodos;
        }

        private async Task<ConselhoClasseComponenteFrequenciaDto> ObterNotasFrequenciaComponente(string componenteCurricularNome, long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar,
            Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool turmaPossuiRegistroFrequencia, bool componenteLancaNota, bool percentualFrequenciaPadrao, bool visualizaNota, string codigoAluno, TurmaComplementarDto turmasComplementares = null)
        {
            var totalAulas = Enumerable.Empty<TotalAulasPorAlunoTurmaDto>();
            var componentePermiteFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(componenteCurricularCodigo));
            var bimestre = periodoEscolar?.Bimestre == null ? 0 : periodoEscolar.Bimestre;
            var percentualFrequencia = double.MinValue;

            if (turmaPossuiRegistroFrequencia)
                percentualFrequencia = Math.Round(frequenciaAluno != null ? frequenciaAluno.PercentualFrequencia : 100);

            if (componentePermiteFrequencia && bimestre == (int)Bimestre.Final)
                totalAulas = await mediator.Send(new ObterTotalAulasPorAlunoTurmaQuery(componenteCurricularCodigo.ToString(), turma.CodigoTurma));
            else if (!componentePermiteFrequencia && bimestre == (int)Bimestre.Final)
                totalAulas = await mediator.Send(new ObterTotalAulasSemFrequenciaPorTurmaQuery(componenteCurricularCodigo.ToString(), turma.CodigoTurma));


            // Cálculo de frequência particular do ano de 2020
            if (periodoEscolar == null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno?.PercentualFrequenciaFinal ?? 0;
            else if (turma.AnoLetivo.Equals(2020) && frequenciaAluno?.TotalAulas == 0)
                percentualFrequencia = 100;

            var conselhoClasseComponente = new ConselhoClasseComponenteFrequenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo,
                QuantidadeAulas = frequenciaAluno?.TotalAulas ?? 0,
                Faltas = frequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = percentualFrequencia < 0 || ((frequenciaAluno?.TotalAulas ?? 0) == 0 && (frequenciaAluno?.TotalAusencias ?? 0) == 0) ? null : percentualFrequencia.ToString(),
                NotasFechamentos = ObterNotasFechamentoOuConselho(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = await ObterNotasPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota, visualizaNota, turmasComplementares),
                Aulas = frequenciaAluno?.TotalAulas.ToString() ?? "0",
            };

            if (!componentePermiteFrequencia)
            {
                if (bimestre == (int)Bimestre.Final)
                    conselhoClasseComponente.Aulas = totalAulas.Count() == 0 ? "0" : totalAulas.FirstOrDefault().TotalAulas;
                else
                {
                    var valor = await mediator.Send(new ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery(componenteCurricularCodigo.ToString(), turma.CodigoTurma, bimestre));
                    conselhoClasseComponente.QuantidadeAulas = valor.FirstOrDefault();
                }
            }

            return conselhoClasseComponente;
        }

        private string ExibirTotalAulas(IEnumerable<TotalAulasPorAlunoTurmaDto> totalAulas, long componenteCurricularCodigo, string codigoAluno)
        {
            var aulasPorAlunoEComponente = totalAulas.Where(x => x.DisciplinaId == componenteCurricularCodigo.ToString() && x.CodigoAluno == codigoAluno);

            if (aulasPorAlunoEComponente == null || !aulasPorAlunoEComponente.Any())
                return "0";

            return aulasPorAlunoEComponente.Sum(x => Convert.ToInt64(x.TotalAulas)).ToString();
        }

        private async Task<ConselhoClasseComponenteRegenciaFrequenciaDto> ObterNotasFrequenciaRegencia(long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno,
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota, bool visualizaNotas)
        {
            var componentesRegencia = await consultasDisciplina.ObterComponentesRegencia(turma);

            if (componentesRegencia == null || !componentesRegencia.Any())
                throw new NegocioException(MensagemNegocioComponentesCurriculares.NAO_FORAM_ENCONTRADOS_COMPONENTES_CURRICULARES_REGENCIA_INFORMADA);

            // Excessão de disciplina ED. Fisica para modalidade EJA
            if (turma.EhEJA())
                componentesRegencia = componentesRegencia.Where(a => a.CodigoComponenteCurricular != 6);

            var percentualFrequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 0 : 0);

            // Cálculo de frequência particular do ano de 2020
            if (periodoEscolar == null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var conselhoClasseComponente = new ConselhoClasseComponenteRegenciaFrequenciaDto()
            {
                QuantidadeAulas = frequenciaAluno.TotalAulas,
                Faltas = frequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = percentualFrequencia <= 0 ? "" : percentualFrequencia.ToString()
            };

            foreach (var componenteRegencia in componentesRegencia)
                conselhoClasseComponente.ComponentesCurriculares.Add(await ObterNotasRegencia(componenteRegencia.Nome, componenteRegencia.CodigoComponenteCurricular, periodoEscolar, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota, visualizaNotas));

            return conselhoClasseComponente;
        }

        private async Task<ConselhoClasseNotasComponenteRegenciaDto> ObterNotasRegencia(string componenteCurricularNome, long componenteCurricularCodigo, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota, bool visualizaNotas)
        {
            return new ConselhoClasseNotasComponenteRegenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo,
                NotasFechamentos = ObterNotasFechamentoOuConselho(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = await ObterNotasPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota, visualizaNotas)
            };
        }

        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestre(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await consultasPeriodoEscolar.ObterUltimoPeriodoAsync(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (periodoEscolarUltimoBimestre == null)
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FOi_ENCONTRADO_PERIODO_ULTIMO_BIMESTRE);

            return periodoEscolarUltimoBimestre;
        }

        public async Task<ParecerConclusivoDto> ObterParecerConclusivoAlunoTurma(string codigoTurma, string alunoCodigo)
        {
            ParecerConclusivoDto parecerConclusivoDto;
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorFiltrosAsync(codigoTurma, alunoCodigo, 0, true);
            if (conselhoClasseAluno is null)
                return new ParecerConclusivoDto();

            if (!turma.EhAnoAnterior() && !conselhoClasseAluno.ConselhoClasseParecerId.HasValue)
                parecerConclusivoDto = await mediator.Send(new GerarParecerConclusivoPorConselhoFechamentoAlunoCommand(
                                        conselhoClasseAluno.ConselhoClasseId,
                                        conselhoClasseAluno.ConselhoClasse.FechamentoTurmaId,
                                        alunoCodigo));
            else
                parecerConclusivoDto = new ParecerConclusivoDto()
                {
                    Id = conselhoClasseAluno?.ConselhoClasseParecerId ?? 0,
                    Nome = conselhoClasseAluno?.ConselhoClasseParecer?.Nome,
                    EmAprovacao = false
                };

            await VerificaEmAprovacaoParecerConclusivo(conselhoClasseAluno?.Id, parecerConclusivoDto);

            return parecerConclusivoDto;
        }

        private long ObterCodigoComponenteCurricular(DisciplinaResposta componenteCurricular)
        {
            return componenteCurricular.TerritorioSaber
                ? componenteCurricular.CodigoComponenteTerritorioSaber.GetValueOrDefault()
                : componenteCurricular.CodigoComponenteCurricular;
        }
    }
}