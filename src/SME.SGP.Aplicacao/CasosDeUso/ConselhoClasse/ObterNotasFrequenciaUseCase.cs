using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFrequenciaUseCase : IObterNotasFrequenciaUseCase
    {
        private readonly IMediator mediator;
        private const int PRIMEIRO_BIMESTRE = 1;

        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;

        public ObterNotasFrequenciaUseCase(IMediator mediator, IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
        }

        public async Task<ConselhoClasseAlunoNotasConceitosRetornoDto> Executar(ConselhoClasseNotasFrequenciaDto notasFrequenciaDto)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(notasFrequenciaDto.CodigoTurma));
            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var turmaItinerarioPercurso = turma.EhTurmaPercurso() ? turma.CodigoTurma : "";

            var anoLetivo = turma.AnoLetivo;
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(notasFrequenciaDto.FechamentoTurmaId, notasFrequenciaDto.AlunoCodigo));
            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma.NaoEhNulo())
                turma = fechamentoTurma?.Turma;
            else if (notasFrequenciaDto.Bimestre > 0)
                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, notasFrequenciaDto.Bimestre));

            bool turmaTipoNotaConceito = await ObterSeATurmaEhTipoNotaConceito(notasFrequenciaDto.AlunoCodigo, turma);
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));

            if (tipoCalendario.EhNulo())
                throw new NegocioException(MensagemNegocioTipoCalendario.TIPO_CALENDARIO_NAO_ENCONTRADO);

            var turmasCodigos = new List<string>();
            long[] conselhosClassesIds;

            var turmasItinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance)).ToList();
            var situacoesAlunoNaTurma = await mediator.Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turma.CodigoTurma), int.Parse(notasFrequenciaDto.AlunoCodigo)));
            var alunoNaTurma = periodoEscolar.NaoEhNulo() ? situacoesAlunoNaTurma.FirstOrDefault(a => a.DataMatricula.Date <= periodoEscolar.PeriodoFim.Date) : situacoesAlunoNaTurma.Last();

            if ((turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
                && !(notasFrequenciaDto.Bimestre == 0 && turma.EhEJA() && !turma.EhTurmaRegular()))
            {
                var tiposParaConsulta = new List<int> { (int)turma.TipoTurma };
                var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();

                tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));
                tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));

                if (alunoNaTurma.NaoEhNulo())
                    notasFrequenciaDto.ConsideraHistorico = alunoNaTurma.Inativo;

                var turmasCodigoAtivos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, notasFrequenciaDto.AlunoCodigo,
                    tiposParaConsulta, periodoEscolar?.PeriodoFim, semestre: turma.Semestre != 0 ? turma.Semestre : null));

                turmasCodigos.AddRange(turmasCodigoAtivos);

                if (!turmasCodigos.Any())
                {
                    turmasCodigos = new List<string>() { turma.CodigoTurma };

                    if (!notasFrequenciaDto.CodigoTurma.Equals(turma.CodigoTurma))
                        turmasCodigos = new List<string>() { notasFrequenciaDto.CodigoTurma, turma.CodigoTurma };
                }
                else if (!turmasCodigos.Contains(turma.CodigoTurma))
                    turmasCodigos.Add(turma.CodigoTurma);

                conselhosClassesIds = await mediator
                    .Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos.Distinct().ToArray(), periodoEscolar?.Id));

                if (conselhosClassesIds.NaoEhNulo() && !conselhosClassesIds.Any())
                    conselhosClassesIds = new long[1] { notasFrequenciaDto.ConselhoClasseId };
            }
            else
            {
                turmasCodigos = new List<string>() { turma.CodigoTurma };

                var conselhosComplementares = await mediator
                    .Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos.ToArray(), periodoEscolar?.Id));

                if (notasFrequenciaDto.ConselhoClasseId > 0 && conselhosComplementares.NaoEhNulo())
                {
                    conselhosClassesIds = conselhosComplementares.NaoEhNulo() ?
                        conselhosComplementares.Append(notasFrequenciaDto.ConselhoClasseId).Distinct().ToArray() : new long[] { notasFrequenciaDto.ConselhoClasseId };
                }
                else if (notasFrequenciaDto.ConselhoClasseId > 0)
                    conselhosClassesIds = new long[] { notasFrequenciaDto.ConselhoClasseId };
                else if (conselhosComplementares.NaoEhNulo())
                    conselhosClassesIds = conselhosComplementares;
                else
                    conselhosClassesIds = null;
            }

            var periodosLetivos = (await mediator
                .Send(new ObterPeriodosEscolaresPorTipoCalendarioQuery(tipoCalendario.Id))).ToList();

            if (periodosLetivos.EhNulo() || !periodosLetivos.Any())
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FORAM_ENCONTRADOS_PERIODOS_TIPO_CALENDARIO);

            var periodoInicio = periodoEscolar?.PeriodoInicio ?? periodosLetivos.OrderBy(pl => pl.Bimestre).First().PeriodoInicio;
            var periodoFim = periodoEscolar?.PeriodoFim ?? periodosLetivos.OrderBy(pl => pl.Bimestre).Last().PeriodoFim;
            var bimestre = periodoEscolar?.Bimestre ?? (int)Bimestre.Final;

            var turmasComMatriculasValidas = await mediator.Send(new ObterTurmasComMatriculasValidasPeriodoQuery(notasFrequenciaDto.AlunoCodigo,
                                                                                                                           turma.EhTurmaInfantil,
                                                                                                                           bimestre,
                                                                                                                           tipoCalendario.Id,
                                                                                                                           turmasCodigos.ToArray(),
                                                                                                                           periodoInicio,
                                                                                                                           periodoFim));
            if (turmasComMatriculasValidas.Any())
                turmasCodigos = turmasComMatriculasValidas.ToList();

            var notasConselhoClasseAluno = new List<NotaConceitoBimestreComponenteDto>();

            if (conselhosClassesIds.NaoEhNulo())
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, notasFrequenciaDto.AlunoCodigo, notasFrequenciaDto.Bimestre));
                    notasConselhoClasseAluno.AddRange(notasParaAdicionar);
                }
            }

            if (turmasCodigos.Count > 0)
                turmasCodigos = await mediator.Send(new ObterTurmasConsideradasNoConselhoQuery(turmasCodigos, turma));

            //Verificar as notas finais
            var notasFechamentoAluno = Enumerable.Empty<NotaConceitoBimestreComponenteDto>();
            var dadosAlunos = await mediator.Send(new ObterDadosAlunosQuery(notasFrequenciaDto.CodigoTurma, turma.AnoLetivo, null, true));
            var periodosEscolares = (await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id))).ToList();

            if (periodosEscolares.NaoEhNulo())
            {
                var dataInicioPrimeiroBimestre = periodosEscolares.FirstOrDefault(pe => pe.Bimestre == PRIMEIRO_BIMESTRE).PeriodoInicio;
                dadosAlunos = dadosAlunos.Where(d => !d.EstaInativo() || d.EstaInativo() && d.SituacaoCodigo != SituacaoMatriculaAluno.VinculoIndevido && d.DataSituacao >= dataInicioPrimeiroBimestre);
            }

            var dadosAluno = dadosAlunos.OrderByDescending(x => x.DataMatricula).FirstOrDefault(da => da.CodigoEOL.Contains(notasFrequenciaDto.AlunoCodigo));

            if (situacoesAlunoNaTurma.Count() > 1)
                dadosAluno = situacoesAlunoNaTurma.Select(d => new AlunoDadosBasicosDto()
                {
                    CodigoEOL = d.CodigoAluno,
                    DataMatricula = d.DataMatricula,
                    DataSituacao = d.DataSituacao,
                    SituacaoCodigo = d.CodigoSituacaoMatricula,
                    NumeroChamada = d.NumeroAlunoChamada ?? 0
                }).OrderByDescending(x => x.DataMatricula).FirstOrDefault(d => d.EstaAtivo() && d.DataMatricula <= periodoFim && d.DataSituacao.Date >= periodoInicio) ?? dadosAluno;

            bool validaMatricula = false;

            if (turmasComMatriculasValidas.Contains(turma.CodigoTurma))
            {
                if (alunoNaTurma.NaoEhNulo())
                    validaMatricula = !MatriculaIgualDataConclusaoAlunoTurma(alunoNaTurma);

                var turmasCodigosFiltro = turmasCodigos.Distinct()
                    .ToArray();

                if (fechamentoTurma.NaoEhNulo() && fechamentoTurma.PeriodoEscolarId.HasValue)
                    notasFechamentoAluno = await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigosFiltro, notasFrequenciaDto.AlunoCodigo, notasFrequenciaDto.Bimestre, dadosAluno.DataMatricula, !dadosAluno.EstaInativo() ? periodoFim : dadosAluno.DataSituacao, anoLetivo));
                else
                    notasFechamentoAluno = await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigosFiltro, notasFrequenciaDto.AlunoCodigo, dadosAluno.DataMatricula, !dadosAluno.EstaInativo() ? periodoFim : dadosAluno.DataSituacao, notasFrequenciaDto.Bimestre, validaMatricula));
            }

            var usuarioAtual = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (!String.IsNullOrEmpty(turmaItinerarioPercurso))
                turmasCodigos.Add(turmaItinerarioPercurso);

            var disciplinasDaTurmaEol =
                (await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigos.ToArray(), usuarioAtual.PerfilAtual,
                    usuarioAtual.Login, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares, true))).ToList();

            if (turmaTipoNotaConceito && turma.EhEJA() && notasFechamentoAluno.Any(x => x.Nota.NaoEhNulo()))
                ConverterNotaFechamentoAlunoNumerica(notasFechamentoAluno);

            var disciplinasCodigo = disciplinasDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray();
            var disciplinasDaTurma = (await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(disciplinasCodigo, codigoTurma: turma.CodigoTurma))).ToList();
            var areasDoConhecimento = (await mediator.Send(new ObterAreasConhecimentoQuery(disciplinasDaTurmaEol))).ToList();
            var ordenacaoGrupoArea = (await mediator.Send(new ObterOrdenacaoAreasConhecimentoQuery(disciplinasDaTurma, areasDoConhecimento))).ToList();
            var retorno = new ConselhoClasseAlunoNotasConceitosRetornoDto();
            var gruposMatrizesNotas = new List<ConselhoClasseAlunoNotasConceitosDto>();

            var matriculaValidasFrequencia = await mediator.Send(new ObterTurmasComMatriculasValidasPeriodoQuery(notasFrequenciaDto.AlunoCodigo,
                                                                                                                           turma.EhTurmaInfantil,
                                                                                                                           bimestre,
                                                                                                                           tipoCalendario.Id,
                                                                                                                           turmasCodigos.ToArray(),
                                                                                                                           periodoInicio,
                                                                                                                           periodoFim,
                                                                                                                           false));

            var frequenciasAluno = matriculaValidasFrequencia.Contains(notasFrequenciaDto.CodigoTurma) && alunoNaTurma.NaoEhNulo() ?
                await ObterFrequenciaAlunoRefatorada(disciplinasDaTurmaEol, periodoEscolar, situacoesAlunoNaTurma, tipoCalendario.Id, notasFrequenciaDto.Bimestre, tipoCalendario.AnoLetivo) :
                Enumerable.Empty<FrequenciaAluno>();

            var frequenciaAlunoRegenciaPai = new FrequenciaAluno();
            if (disciplinasDaTurmaEol.Any(x => x.Regencia))
            {
                var componenteRegenciaPai = disciplinasDaTurmaEol.FirstOrDefault(d => d.Regencia)?.CdComponenteCurricularPai.ToString();
                if (frequenciasAluno.Any())
                {
                    frequenciaAlunoRegenciaPai = frequenciasAluno.FirstOrDefault(f => f.DisciplinaId == componenteRegenciaPai);
                    if (!(frequenciaAlunoRegenciaPai is null))
                    {
                        frequenciaAlunoRegenciaPai.TotalAulas = frequenciasAluno.Where(f => f.DisciplinaId == componenteRegenciaPai).Sum(s => s.TotalAulas);
                        frequenciaAlunoRegenciaPai.TotalAusencias = frequenciasAluno.Where(f => f.DisciplinaId == componenteRegenciaPai).Sum(s => s.TotalAusencias);
                        frequenciaAlunoRegenciaPai.TotalCompensacoes = frequenciasAluno.Where(f => f.DisciplinaId == componenteRegenciaPai).Sum(s => s.TotalCompensacoes);
                    }
                }
            }

            var registrosFrequencia = (turmasComMatriculasValidas.Contains(notasFrequenciaDto.CodigoTurma) && alunoNaTurma.NaoEhNulo() ?
                await mediator.Send(new ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery(notasFrequenciaDto.AlunoCodigo, turmasCodigos.ToArray(),
                    disciplinasCodigo.Select(d => d.ToString()).ToArray(), periodoEscolar?.Id)) :
                Enumerable.Empty<RegistroFrequenciaAlunoBimestreDto>()).ToList();

            var gruposMatrizes = disciplinasDaTurma
                .Where(c => c.GrupoMatrizNome.NaoEhNulo() && c.LancaNota)
                .OrderBy(d => d.GrupoMatrizId)
                .GroupBy(c => c.GrupoMatrizId)
                .ToList();

            var permiteEdicao = (periodoEscolar is null && dadosAluno.EstaAtivo()) ||
                (dadosAluno.EstaAtivo() && dadosAluno.DataMatricula.Date <= periodoFim) ||
                (!dadosAluno.EstaAtivo() && dadosAluno.DataSituacao.Date > periodoInicio);

            var periodoMatricula = alunoNaTurma.NaoEhNulo() ? await mediator
                .Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(tipoCalendario.Id, alunoNaTurma.DataMatricula)) : null;

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
                        var disciplinaEol = disciplinasDaTurmaEol.FirstOrDefault(d => d.CodigoComponenteCurricular == disciplina.CodigoComponenteCurricular);

                        var dataFim = periodoEscolar?.PeriodoFim ?? periodoFim;
                        var dataInicio = periodoEscolar?.PeriodoInicio ?? periodoInicio;

                        var frequenciasAlunoParaTratar = frequenciasAluno.Where(a => (a.DisciplinaId == disciplina.Id.ToString() ||
                                                                                      a.DisciplinaId == disciplina.CodigoComponenteCurricular.ToString()) &&
                                                         situacoesAlunoNaTurma.Select(d => new { d.DataMatricula, d.DataSituacao, d.Ativo })
                                                         .Any(a => (a.Ativo && a.DataMatricula < dataFim
                                                         || MatriculaIgualDataConclusaoAlunoTurma(alunoNaTurma))
                                                         || !a.Ativo && a.DataMatricula < dataFim && a.DataSituacao > dataInicio))?.ToList();

                        frequenciasAlunoParaTratar = (periodoMatricula.NaoEhNulo() && situacoesAlunoNaTurma.Count() == 1 ? frequenciasAlunoParaTratar.Where(f => periodoMatricula.Bimestre <= f.Bimestre) : frequenciasAlunoParaTratar).ToList();

                        FrequenciaAluno frequenciaAluno;

                        if (frequenciasAlunoParaTratar.EhNulo() || !frequenciasAlunoParaTratar.Any())
                            frequenciaAluno = new FrequenciaAluno() { DisciplinaId = disciplina.CodigoComponenteCurricular.ToString(), TurmaId = disciplinaEol.TurmaCodigo };
                        else if (frequenciasAlunoParaTratar.Count == 1)
                        {
                            frequenciaAluno = frequenciasAlunoParaTratar.FirstOrDefault();
                            if (frequenciasAlunoParaTratar.FirstOrDefault().TotalPresencas == 0 && frequenciasAlunoParaTratar.FirstOrDefault().TotalAusencias == 0 && notasFrequenciaDto.Bimestre != 0)
                            {
                                int totalCompensacoesAVerificar = await TotalCompensacoesEFaltasAlunoAVerificar(notasFrequenciaDto.CodigoTurma, notasFrequenciaDto.AlunoCodigo, notasFrequenciaDto.Bimestre, disciplina.CodigoComponenteCurricular.ToString());
                                frequenciaAluno.TotalCompensacoes = totalCompensacoesAVerificar;
                                frequenciaAluno.TotalPresencas = frequenciaAluno.TotalAulas - totalCompensacoesAVerificar;
                            }
                        }
                        else
                        {
                            frequenciaAluno = new FrequenciaAluno
                            {
                                DisciplinaId = disciplina.CodigoComponenteCurricular.ToString(),
                                CodigoAluno = notasFrequenciaDto.AlunoCodigo,
                                TurmaId = turma.CodigoTurma,
                                TotalAulas = frequenciasAlunoParaTratar.Sum(a => a.TotalAulas),
                                TotalAusencias = frequenciasAlunoParaTratar.Sum(a => a.TotalAusencias),
                                TotalCompensacoes = frequenciasAlunoParaTratar.Sum(a => a.TotalCompensacoes)
                            };

                            frequenciasAlunoParaTratar
                                .ToList()
                                .ForEach(f =>
                                {
                                    frequenciaAluno
                                        .AdicionarFrequenciaBimestre(f.Bimestre, tipoCalendario.AnoLetivo.Equals(2020) && f.TotalAulas.Equals(0) ? 100 : f.PercentualFrequencia);
                                });
                        }

                        var notaFrequencia = new NotaFrequenciaDto(disciplina.CodigoComponenteCurricular,
                                frequenciaAluno, periodoEscolar, turma, notasConselhoClasseAluno, notasFechamentoAluno, disciplina.LancaNota,
                                permiteEdicao, turmasCodigos.Distinct().ToArray(), frequenciaAlunoRegenciaPai);

                        if (disciplinaEol.Regencia)
                        {
                            conselhoClasseAlunoNotas.ComponenteRegencia = await ObterNotasFrequenciaRegencia(notaFrequencia);
                        }
                        else
                        {
                            var turmaPossuiRegistroFrequencia = VerificarSePossuiRegistroFrequencia(notasFrequenciaDto.AlunoCodigo, disciplinaEol.TurmaCodigo,
                                disciplina.CodigoComponenteCurricular, periodoEscolar, frequenciasAlunoParaTratar, registrosFrequencia);

                            conselhoClasseAlunoNotas.ComponentesCurriculares.Add(await ObterNotasFrequenciaComponente(disciplina.Nome, turmaPossuiRegistroFrequencia, dadosAluno.DataMatricula, notaFrequencia));
                        }
                    }
                }

                if (conselhoClasseAlunoNotas.ComponentesCurriculares.Any())
                    conselhoClasseAlunoNotas.ComponentesCurriculares = conselhoClasseAlunoNotas.ComponentesCurriculares.OrderBy(c => c.Nome).ToList();

                if (conselhoClasseAlunoNotas.ComponenteRegencia.NaoEhNulo())
                    conselhoClasseAlunoNotas.ComponenteRegencia.ComponentesCurriculares = conselhoClasseAlunoNotas.ComponenteRegencia.ComponentesCurriculares.OrderBy(c => c.Nome).ToList();

                if(conselhoClasseAlunoNotas.ComponenteRegencia.NaoEhNulo() && turma.Ue.TipoEscola != TipoEscola.EMEBS && (TipoTurnoEOL)turma.TipoTurno != TipoTurnoEOL.Integral)
                {
                    conselhoClasseAlunoNotas.ComponenteRegencia.ComponentesCurriculares = conselhoClasseAlunoNotas.ComponenteRegencia.ComponentesCurriculares.Where(d => d.CodigoComponenteCurricular != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_LIBRAS).ToList();
                }

                gruposMatrizesNotas.Add(conselhoClasseAlunoNotas);
            }

            retorno.TemConselhoClasseAluno = notasFrequenciaDto.ConselhoClasseId > 0 && await VerificaSePossuiConselhoClasseAlunoAsync(notasFrequenciaDto.ConselhoClasseId, notasFrequenciaDto.AlunoCodigo);

            var periodoEscolarParaEdicaoNota = periodoEscolar ?? periodosLetivos.OrderByDescending(p => p.Bimestre).FirstOrDefault();

            retorno.PodeEditarNota = permiteEdicao && await this.mediator.Send(new VerificaSePodeEditarNotaQuery(notasFrequenciaDto.AlunoCodigo, turma, periodoEscolarParaEdicaoNota));
            retorno.NotasConceitos = gruposMatrizesNotas;
            retorno.DadosArredondamento = await mediator.Send(new ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery(periodoFim));
            return retorno;
        }

        private async Task<bool> ObterSeATurmaEhTipoNotaConceito(string alunoCodigo, Turma turma)
        {
            var turmaAluno = turma;
            var turmasitinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance)).ToList();
            if (turmaAluno.EhTurmaEdFisicaOuItinerario() || turmasitinerarioEnsinoMedio.Any(a => a.Id == (int)turmaAluno.TipoTurma))
            {
                var turmasCodigosParaConsulta = new List<int>();
                turmasCodigosParaConsulta.AddRange(turmaAluno.ObterTiposRegularesDiferentes());

                var codigosTurmasRelacionadas = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turmaAluno.AnoLetivo, alunoCodigo, turmasCodigosParaConsulta, semestre: turmaAluno.Semestre != 0 ? turmaAluno.Semestre : null));

                turmaAluno = await mediator.Send(new ObterTurmaPorCodigoQuery(codigosTurmasRelacionadas.FirstOrDefault()));
            }
            var ultimoBimestre = await ObterPeriodoUltimoBimestrePorTurma(turmaAluno);
            var periodoFechamentoBimestre = await consultasPeriodoFechamento
                .TurmaEmPeriodoDeFechamentoVigente(turmaAluno, DateTimeExtension.HorarioBrasilia().Date, ultimoBimestre.Bimestre);
            var tipoNota = await ObterTipoNota(turmaAluno, periodoFechamentoBimestre);

            return tipoNota == TipoNota.Conceito;
        }
        private async Task<TipoNota> ObterTipoNota(Turma turma, PeriodoFechamentoVigenteDto periodoFechamentoVigente)
        {
            var dataReferencia = periodoFechamentoVigente?.PeriodoFechamentoFim ?? (await ObterPeriodoUltimoBimestrePorTurma(turma)).PeriodoFim;
            return await mediator.Send(new ObterTipoNotaPorTurmaQuery(turma,dataReferencia));
        }
        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestrePorTurma(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await mediator.Send(new ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            if (periodoEscolarUltimoBimestre.EhNulo())
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FOI_ENCONTRADO_PERIODO_ULTIMO_BIMESTRE);

            return periodoEscolarUltimoBimestre;
        }

        private bool VerificarSePossuiRegistroFrequencia(string alunoCodigo, string turmaCodigo, long codigoComponenteCurricular, PeriodoEscolar periodoEscolar, IEnumerable<FrequenciaAluno> frequenciasAlunoParaTratar, IEnumerable<RegistroFrequenciaAlunoBimestreDto> registrosFrequencia)
        {
            return (frequenciasAlunoParaTratar.NaoEhNulo() && frequenciasAlunoParaTratar.Any()) ||
                   registrosFrequencia.Any(f => (periodoEscolar.EhNulo() || f.Bimestre == periodoEscolar.Bimestre) &&
                                                f.CodigoAluno == alunoCodigo &&
                                                f.CodigoComponenteCurricular == codigoComponenteCurricular &&
                                                f.CodigoTurma == turmaCodigo);
        }
        
        private async Task<ConselhoClasseComponenteRegenciaFrequenciaDto> ObterNotasFrequenciaRegencia(NotaFrequenciaDto dto)
        {
            var componentesRegencia = await mediator.Send(new ObterComponentesRegenciaPorAnoQuery(dto.Turma.TipoTurno == 4 || dto.Turma.TipoTurno == 5 ? dto.Turma.AnoTurmaInteiro : 0));

            if (componentesRegencia.EhNulo() || !componentesRegencia.Any())
                throw new NegocioException(MensagemNegocioComponentesCurriculares.NAO_FORAM_ENCONTRADOS_COMPONENTES_CURRICULARES_REGENCIA_INFORMADA);

            // Excessão de disciplina ED. Fisica para modalidade EJA
            if (dto.Turma.EhEJA())
                componentesRegencia = componentesRegencia.Where(a => a.CodigoComponenteCurricular != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA);

            double percentualFrequencia;

            if(componentesRegencia.NaoEhNulo() && componentesRegencia.Any() && dto.FrequenciaAlunoRegenciaPai.NaoEhNulo())
                percentualFrequencia = (dto.FrequenciaAlunoRegenciaPai.TotalAulas > 0 ? dto.FrequenciaAlunoRegenciaPai?.PercentualFrequencia ?? 0 : 0);
            else 
                percentualFrequencia = (dto.FrequenciaAluno.TotalAulas > 0 ? dto.FrequenciaAluno?.PercentualFrequencia ?? 0 : 0);

            // Cálculo de frequência particular do ano de 2020
            if (dto.PeriodoEscolar.EhNulo() && dto.Turma.AnoLetivo.Equals(2020))
                percentualFrequencia = dto.FrequenciaAluno.PercentualFrequenciaFinal;

            var conselhoClasseComponente = new ConselhoClasseComponenteRegenciaFrequenciaDto()
            {
                QuantidadeAulas = componentesRegencia.NaoEhNulo() && componentesRegencia.Any() ? dto.FrequenciaAlunoRegenciaPai?.TotalAulas ?? 0 : dto.FrequenciaAluno.TotalAulas,
                Faltas = componentesRegencia.NaoEhNulo() && componentesRegencia.Any() ? dto.FrequenciaAlunoRegenciaPai?.TotalAusencias ?? 0 : dto.FrequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = componentesRegencia.NaoEhNulo() && componentesRegencia.Any() ? dto.FrequenciaAlunoRegenciaPai?.TotalCompensacoes ?? 0  : dto.FrequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = percentualFrequencia <= 0 ? "" : FrequenciaAluno.FormatarPercentual(percentualFrequencia)
            };

            foreach (var componenteRegencia in componentesRegencia)
                conselhoClasseComponente.ComponentesCurriculares.Add(await ObterNotasRegencia(componenteRegencia.Nome, componenteRegencia.CodigoComponenteCurricular, dto));

            return conselhoClasseComponente;
        }
        
        private async Task<ConselhoClasseNotasComponenteRegenciaDto> ObterNotasRegencia(string componenteCurricularNome, long componenteCurricularCodigo, NotaFrequenciaDto dto)
        {
            return new ConselhoClasseNotasComponenteRegenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo,
                NotasFechamentos = ObterNotasFechamentoOuConselho(componenteCurricularCodigo, dto.PeriodoEscolar, dto.NotasFechamentoAluno),
                NotaPosConselho = await ObterNotasPosConselho(componenteCurricularCodigo, dto)
            };
        }

        private NotaConceitoBimestreComponenteDto ObterNotaConselhoComponenteTurma(long componenteCurricularCodigo, NotaFrequenciaDto dto)
        {
            return dto.NotasConselhoClasseAluno.OrderByDescending(x => x.ConselhoClasseNotaId).FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo
                                        && dto.CodigosTurma.Contains(c.TurmaCodigo));
        }

        private bool NaoContemNotaComponenteTurma(NotaConceitoBimestreComponenteDto notaFrequencia)
        {
            return notaFrequencia.EhNulo() || !notaFrequencia.NotaConceito.HasValue;
        }

        private NotaConceitoBimestreComponenteDto ObterNotaFechamentoComponenteTurma(long componenteCurricularCodigo, NotaFrequenciaDto dto)
        {
            if (dto.Turma.EhEJA() || dto.Turma.EhTurmaEnsinoMedio)
                return dto.NotasFechamentoAluno.FirstOrDefault(t => dto.CodigosTurma.Contains(t.TurmaCodigo) && t.ComponenteCurricularCodigo == componenteCurricularCodigo && t.Bimestre == dto.PeriodoEscolar?.Bimestre && t.ConselhoClasseNotaId > 0)
                       ?? dto.NotasFechamentoAluno.FirstOrDefault(t => dto.CodigosTurma.Contains(t.TurmaCodigo) && t.ComponenteCurricularCodigo == componenteCurricularCodigo && t.Bimestre == dto.PeriodoEscolar?.Bimestre);
            
            var notasFechamentoAluno = dto.NotasFechamentoAluno.Select(n => n.TurmaCodigo).Distinct().Count() > 1 ? dto.NotasFechamentoAluno.Where(n => n.TurmaCodigo == dto.Turma.CodigoTurma) : dto.NotasFechamentoAluno;
            return notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == dto.PeriodoEscolar?.Bimestre && c.ConselhoClasseNotaId > 0)
                    ?? notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == dto.PeriodoEscolar?.Bimestre);
        }

        private async Task<NotaPosConselhoDto> ObterNotasPosConselho(long componenteCurricularCodigo, NotaFrequenciaDto dto)
        {
            // Busca nota do conselho de classe consultado
            var notaComponente = ObterNotaConselhoComponenteTurma(componenteCurricularCodigo, dto);
            var notaComponenteId = notaComponente?.ConselhoClasseNotaId;

            if (NaoContemNotaComponenteTurma(notaComponente))
                notaComponente = ObterNotaFechamentoComponenteTurma(componenteCurricularCodigo, dto);

            var notaPosConselho = new NotaPosConselhoDto()
            {
                Id = notaComponenteId,
                Nota = notaComponente?.NotaConceito,
                PodeEditar = dto.ComponenteLancaNota && dto.VisualizaNotas
            };

            if (notaComponenteId.HasValue)
                await VerificaNotaEmAprovacao(notaComponenteId.Value, notaPosConselho);

            return notaPosConselho;
        }

        private async Task VerificaNotaEmAprovacao(long conselhoClasseNotaId, NotaPosConselhoDto nota)
        {
            double? notaConselhoEmAprovacao = await mediator.Send(new ObterNotaConselhoEmAprovacaoQuery(conselhoClasseNotaId));

            if (notaConselhoEmAprovacao.HasValue && notaConselhoEmAprovacao >= 0)
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

            if (periodoEscolar.NaoEhNulo())
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

        private NotaBimestreDto ObterNotasFinaisFechamento(long codigoComponenteCurricular, int bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            double? notaConceito = null;
            // Busca nota do FechamentoNota
            var notaFechamento = notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == codigoComponenteCurricular);
            if (notaFechamento.NaoEhNulo())
                notaConceito = notaFechamento.NotaConceito;

            return new NotaBimestreDto()
            {
                Bimestre = bimestre,
                NotaConceito = notaConceito,
            };
        }

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunoRefatorada(IEnumerable<DisciplinaDto> disciplinasDaTurma, PeriodoEscolar periodoEscolar, IEnumerable<AlunoPorTurmaResposta> situacoesAlunoNaTurma,
            long tipoCalendarioId, int bimestre, int anoLetivo = 0)
        {
            var frequenciasAlunoRetorno = new List<FrequenciaAluno>();
            string codigoAluno = situacoesAlunoNaTurma.FirstOrDefault().CodigoAluno;
            var disciplinasId = disciplinasDaTurma.Select(a => a.CodigoComponenteCurricular.ToString()).Distinct().ToList();
            var disciplinaIdRegenciaPai = disciplinasDaTurma.FirstOrDefault(d => d.Regencia && d.CdComponenteCurricularPai != 0)?.CdComponenteCurricularPai;
            if (disciplinaIdRegenciaPai.HasValue)
                disciplinasId.Add(disciplinaIdRegenciaPai.ToString());
            var disciplinas = disciplinasId.ToArray();

            var turmasCodigo = disciplinasDaTurma.Select(a => a.TurmaCodigo).Distinct().ToArray();

            int[] bimestres;
            if (periodoEscolar.EhNulo())
            {
                var periodosEscolaresTurma = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

                if (periodosEscolaresTurma.Any())
                    bimestres = periodosEscolaresTurma.Select(a => a.Bimestre).ToArray();
                else throw new NegocioException(MensagemNegocioPeriodo.NAO_FORAM_ENCONTRADOS_PERIODOS_TURMA);
            }
            else bimestres = new int[] { bimestre };

            var frequenciasAluno = await mediator.Send(new ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQuery(codigoAluno,
                                                         TipoFrequenciaAluno.PorDisciplina,
                                                         disciplinas,
                                                         turmasCodigo, bimestres));

            var aulasComponentesTurmas = await mediator
                .Send(new ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery(turmasCodigo, tipoCalendarioId, disciplinas, bimestres));

            if (aulasComponentesTurmas.NaoEhNulo())
                aulasComponentesTurmas = aulasComponentesTurmas.Where(a => situacoesAlunoNaTurma.Select(s => new { s.DataMatricula, s.DataSituacao, s.Ativo })
                                                                           .Any(d => d.Ativo && d.DataMatricula < a.PeriodoFim ||
                                                                                !d.Ativo && d.DataMatricula < a.PeriodoFim && d.DataSituacao > a.PeriodoInicio))?.ToList();

            if (frequenciasAluno.NaoEhNulo() && frequenciasAluno.Any())
                frequenciasAlunoRetorno.AddRange(frequenciasAluno);

            foreach (var aulaComponenteTurma in aulasComponentesTurmas)
            {
                if (!frequenciasAlunoRetorno.Any(a => a.TurmaId == aulaComponenteTurma.TurmaCodigo && a.DisciplinaId == aulaComponenteTurma.ComponenteCurricularCodigo && a.Bimestre == aulaComponenteTurma.Bimestre))
                {
                    frequenciasAlunoRetorno.Add(new FrequenciaAluno()
                    {
                        CodigoAluno = codigoAluno,
                        DisciplinaId = aulaComponenteTurma.ComponenteCurricularCodigo,
                        TurmaId = aulaComponenteTurma.TurmaCodigo,
                        TotalAulas = 0,
                        Bimestre = aulaComponenteTurma.Bimestre,
                        PeriodoEscolarId = aulaComponenteTurma.PeriodoEscolarId
                    });
                }
            }

            // Cálculo específico para 2020
            if (periodoEscolar.EhNulo() && anoLetivo.Equals(2020))
            {
                var frequenciasAlunoRetornoAgrupado = frequenciasAlunoRetorno
                    .GroupBy(fa => (fa.TurmaId, fa.CodigoAluno, fa.DisciplinaId))
                    .ToList();

                foreach (var frequenciasAlunoAtual in frequenciasAlunoRetornoAgrupado)
                {
                    foreach (var bimestreAtual in bimestres)
                    {
                        foreach (var frequenciaAlunoAtual in frequenciasAlunoAtual)
                        {
                            frequenciaAlunoAtual.PercentuaisFrequenciaPorBimestre
                                .Add((bimestreAtual,
                                    frequenciasAlunoAtual.Any(f => f.Bimestre == bimestreAtual)
                                        ? frequenciasAlunoAtual.FirstOrDefault(f => f.Bimestre == bimestreAtual).PercentualFrequencia
                                        : 0));
                        }
                    }
                }
            }

            return frequenciasAlunoRetorno;
        }

        private async Task<int> TotalCompensacoesEFaltasAlunoAVerificar(string turmaCodigo, string codigoAluno, int bimestre, string disciplinaCodigo)
        {
            var compensacoes = await mediator.Send(new ObterTotalCompensacoesAlunosETurmaPorPeriodoQuery(bimestre, new List<string>() { codigoAluno }, turmaCodigo));
            if (compensacoes.Any())
            {
                var compensacoesDisciplina = compensacoes.FirstOrDefault(c => c.ComponenteCurricularId == disciplinaCodigo);

                if (compensacoesDisciplina.NaoEhNulo())
                    return compensacoesDisciplina.Compensacoes;
            }
            return 0;
        }

        private void ConverterNotaFechamentoAlunoNumerica(IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            foreach (var notaFechamento in notasFechamentoAluno)
            {
                if (MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA.Equals(notaFechamento.ComponenteCurricularCodigo)
                    && notaFechamento.Nota.NaoEhNulo())
                {
                    notaFechamento.ConceitoId = ObterIdConceito(notaFechamento.Nota);
                }
            }
        }

        private int ObterIdConceito(double? nota)
        {
            if (nota >= 7)
                return (int)ConceitoValores.P;

            if (nota >= 5 && nota <= 7)
                return (int)ConceitoValores.S;

            return (int)ConceitoValores.NS;
        }

        private bool MatriculaIgualDataConclusaoAlunoTurma(AlunoPorTurmaResposta alunoNaTurma)
        {
            return alunoNaTurma.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido && alunoNaTurma.DataMatricula.Date == alunoNaTurma.DataSituacao.Date;
        }

        private double ObterPercentualFrequencia(bool turmaPossuiRegistroFrequencia, NotaFrequenciaDto dto)
        {
            if (dto.PeriodoEscolar.EhNulo() && dto.Turma.AnoLetivo.Equals(2020))
                return dto.FrequenciaAluno?.PercentualFrequenciaFinal ?? 0;
            if (dto.Turma.AnoLetivo.Equals(2020) && dto.FrequenciaAluno?.TotalAulas == 0)
                return  100;
            if (turmaPossuiRegistroFrequencia && dto.FrequenciaAluno.NaoEhNulo())
                return dto.FrequenciaAluno.PercentualFrequencia;
            return double.MinValue;
        }

        private async Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> ObterTotalAulas(bool componentePermiteFrequencia, int bimestre, NotaFrequenciaDto dto)
        {
            if (componentePermiteFrequencia && bimestre == (int)Bimestre.Final)
                return await mediator.Send(new ObterTotalAulasPorAlunoTurmaQuery(dto.ComponenteCurricularCodigo.ToString(), dto.Turma.CodigoTurma));
            if (!componentePermiteFrequencia && bimestre == (int)Bimestre.Final)
                return await mediator.Send(new ObterTotalAulasSemFrequenciaPorTurmaQuery(dto.ComponenteCurricularCodigo.ToString(), dto.Turma.CodigoTurma));

            return Enumerable.Empty<TotalAulasPorAlunoTurmaDto>(); 
        }

        private string ObterPercFrequenciaFormatado(double percentualFrequencia, NotaFrequenciaDto dto)
        {
            var frequenciaInvalida = percentualFrequencia < 0;
            var semRegistroAulas = ((dto.FrequenciaAluno?.TotalAulas ?? 0) == 0 && (dto.FrequenciaAluno?.TotalAusencias ?? 0) == 0);
            return frequenciaInvalida || semRegistroAulas 
                   ? null 
                   : FrequenciaAluno.FormatarPercentual(percentualFrequencia);
        }

        private async Task<int> ObterQuantidadeAulas(string componenteCurricularCodigo, string turmaCodigo, int bimestre, DateTime dataMatricula)
        {
            var valor = await mediator.Send(new ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery(componenteCurricularCodigo, turmaCodigo, bimestre, dataMatricula));
            return valor.FirstOrDefault();
        }

        private async Task<ConselhoClasseComponenteFrequenciaDto> ObterNotasFrequenciaComponente(string componenteCurricularNome, 
                                                                                                 bool turmaPossuiRegistroFrequencia, 
                                                                                                 DateTime dataMatricula, 
                                                                                                 NotaFrequenciaDto dto)
        {
            var componentePermiteFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(dto.ComponenteCurricularCodigo));
            var bimestre = dto.PeriodoEscolar?.Bimestre ?? 0;
            var percentualFrequencia = ObterPercentualFrequencia(turmaPossuiRegistroFrequencia, dto);
            var totalAulas = await ObterTotalAulas(componentePermiteFrequencia, bimestre, dto);
            

            var conselhoClasseComponente = new ConselhoClasseComponenteFrequenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = dto.ComponenteCurricularCodigo,
                QuantidadeAulas = dto.FrequenciaAluno?.TotalAulas ?? 0,
                Faltas = dto.FrequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = dto.FrequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = ObterPercFrequenciaFormatado(percentualFrequencia, dto),
                NotasFechamentos = ObterNotasFechamentoOuConselho(dto.ComponenteCurricularCodigo, dto.PeriodoEscolar, dto.NotasFechamentoAluno),
                NotaPosConselho = await ObterNotasPosConselho(dto.ComponenteCurricularCodigo, dto),
                Aulas = dto.FrequenciaAluno?.TotalAulas.ToString() ?? "0",
            };

            if (!componentePermiteFrequencia)         
                if (bimestre == (int)Bimestre.Final)
                    conselhoClasseComponente.Aulas = totalAulas.FirstOrDefault()?.TotalAulas ?? "0";
                else
                    conselhoClasseComponente.QuantidadeAulas = await ObterQuantidadeAulas(dto.ComponenteCurricularCodigo.ToString(), dto.Turma.CodigoTurma, bimestre, dataMatricula.Date);

            return conselhoClasseComponente;
        }

        private async Task<bool> VerificaSePossuiConselhoClasseAlunoAsync(long conselhoClasseId, string alunoCodigo)
        {
            var conselhoClasseAlunoId = await mediator.Send(new ObterConselhoClasseAlunoIdQuery(conselhoClasseId, alunoCodigo));
            return conselhoClasseAlunoId > 0;
        }

    }
}