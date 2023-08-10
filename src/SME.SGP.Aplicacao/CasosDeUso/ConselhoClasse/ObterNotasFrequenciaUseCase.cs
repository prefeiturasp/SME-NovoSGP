using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ConselhoClasse;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFrequenciaUseCase : IObterNotasFrequenciaUseCase
    {
        private readonly IMediator mediator;
        private const int PRIMEIRO_BIMESTRE = 1;
        private const string PRIMEIRO_ANO_EM = "1";
        private const double NOTA_CONCEITO_CINCO = 5.0;
        private const double NOTA_CONCEITO_SETE = 7.0;
        

        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;

        public ObterNotasFrequenciaUseCase(IMediator mediator,IConsultasPeriodoFechamento consultasPeriodoFechamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
        }

        public async Task<ConselhoClasseAlunoNotasConceitosRetornoDto> Executar(ConselhoClasseNotasFrequenciaDto notasFrequenciaDto)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(notasFrequenciaDto.CodigoTurma));
            bool turmaInicialEhEdFisica = turma.EhTurmaEdFisica();
            if (turma == null)
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var turmaItinerarioPercurso = turma.EhTurmaPercurso() ? turma.CodigoTurma : "";

            var anoLetivo = turma.AnoLetivo;
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(notasFrequenciaDto.FechamentoTurmaId, notasFrequenciaDto.AlunoCodigo, notasFrequenciaDto.ConsideraHistorico));
            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma != null)
                turma = fechamentoTurma?.Turma;
            else if (notasFrequenciaDto.Bimestre > 0)
                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, notasFrequenciaDto.Bimestre));

            bool turmaTipoNotaConceito = await ObterSeATurmaEhTipoNotaConceito(notasFrequenciaDto.AlunoCodigo, turma);
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));

            if (tipoCalendario == null)
                throw new NegocioException(MensagemNegocioTipoCalendario.TIPO_CALENDARIO_NAO_ENCONTRADO);

            var turmasCodigos = new List<string>();
            long[] conselhosClassesIds;

            var turmasItinerarioEnsinoMedio = (await mediator.Send(ObterTurmaItinerarioEnsinoMedioQuery.Instance)).ToList();
            var alunosEol = await mediator.Send(new ObterTodosAlunosNaTurmaQuery(int.Parse(turma.CodigoTurma), int.Parse(notasFrequenciaDto.AlunoCodigo)));
            var alunoNaTurma = periodoEscolar != null ? alunosEol.FirstOrDefault(a => a.DataMatricula.Date <= periodoEscolar.PeriodoFim.Date) : alunosEol.Last();
            var codigosAlunos = alunosEol.Select(a => a.CodigoAluno).ToArray();

            if ((turma.DeveVerificarRegraRegulares() || turmasItinerarioEnsinoMedio.Any(a => a.Id == (int)turma.TipoTurma))
                && !(notasFrequenciaDto.Bimestre == 0 && turma.EhEJA() && !turma.EhTurmaRegular()))
            {
                var tiposParaConsulta = new List<int> { (int)turma.TipoTurma };
                var tiposRegularesDiferentes = turma.ObterTiposRegularesDiferentes();

                tiposParaConsulta.AddRange(tiposRegularesDiferentes.Where(c => tiposParaConsulta.All(x => x != c)));
                tiposParaConsulta.AddRange(turmasItinerarioEnsinoMedio.Select(s => s.Id).Where(c => tiposParaConsulta.All(x => x != c)));

                if (alunoNaTurma != null)
                    notasFrequenciaDto.ConsideraHistorico = alunoNaTurma.Inativo;

                var turmasCodigoAtivos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, notasFrequenciaDto.AlunoCodigo,
                    tiposParaConsulta, notasFrequenciaDto.ConsideraHistorico));

                turmasCodigos.AddRange(turmasCodigoAtivos.ToList());

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

                if (conselhosClassesIds != null && !conselhosClassesIds.Any())
                    conselhosClassesIds = new long[1] { notasFrequenciaDto.ConselhoClasseId };
            }
            else
            {
                turmasCodigos = new List<string>() { turma.CodigoTurma };

                var conselhosComplementares = await mediator
                    .Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos.ToArray(), periodoEscolar?.Id));

                if (notasFrequenciaDto.ConselhoClasseId > 0 && conselhosComplementares != null)
                {
                    conselhosClassesIds = conselhosComplementares != null ?
                        conselhosComplementares.Append(notasFrequenciaDto.ConselhoClasseId).Distinct().ToArray() : new long[] { notasFrequenciaDto.ConselhoClasseId };
                }
                else if (notasFrequenciaDto.ConselhoClasseId > 0)
                    conselhosClassesIds = new long[] { notasFrequenciaDto.ConselhoClasseId };
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

            var turmasComMatriculasValidas = await mediator.Send(new ObterTurmasComMatriculasValidasQuery(notasFrequenciaDto.AlunoCodigo, turmasCodigos.ToArray(), periodoInicio, periodoFim));

            if (turmasComMatriculasValidas.Any())
                turmasCodigos = turmasComMatriculasValidas.ToList();

            var notasConselhoClasseAluno = new List<NotaConceitoBimestreComponenteDto>();

            if (conselhosClassesIds != null)
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await mediator.Send(new ObterConselhoClasseNotasAlunoQuery(conselhosClassesId, notasFrequenciaDto.AlunoCodigo, notasFrequenciaDto.Bimestre));
                    notasConselhoClasseAluno.AddRange(notasParaAdicionar);
                }
            }

            if (turmasCodigos.Count > 0)
            {
                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos.ToArray()));

                if (turmas.Select(t => t.TipoTurma).Distinct().Count() == 1 && turma.ModalidadeCodigo != Modalidade.Medio)
                    turmasCodigos = new List<string>() { turma.CodigoTurma };
                else if (ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(turmas, turma))
                    turmasCodigos = new List<string>() { turma.CodigoTurma };
            }

            //Verificar as notas finais
            var notasFechamentoAluno = Enumerable.Empty<NotaConceitoBimestreComponenteDto>();
            var dadosAlunos = await mediator.Send(new ObterDadosAlunosQuery(notasFrequenciaDto.CodigoTurma, turma.AnoLetivo, null, true));
            var periodosEscolares = (await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendario.Id))).ToList();

            if (periodosEscolares != null)
            {
                var dataInicioPrimeiroBimestre = periodosEscolares.FirstOrDefault(pe => pe.Bimestre == PRIMEIRO_BIMESTRE).PeriodoInicio;
                dadosAlunos = dadosAlunos.Where(d => !d.EstaInativo() || d.EstaInativo() && d.SituacaoCodigo != SituacaoMatriculaAluno.VinculoIndevido && d.DataSituacao >= dataInicioPrimeiroBimestre);
            }

            var dadosAluno = dadosAlunos.FirstOrDefault(da => da.CodigoEOL.Contains(notasFrequenciaDto.AlunoCodigo));
            var dadosMatriculaAlunoNaTurma = await mediator.Send(new ObterMatriculasAlunoNaTurmaQuery(turma.CodigoTurma, notasFrequenciaDto.AlunoCodigo));

            if (dadosMatriculaAlunoNaTurma.Count() > 1)
                dadosAluno = dadosMatriculaAlunoNaTurma.Select(d => new AlunoDadosBasicosDto()
                {
                    CodigoEOL = d.CodigoAluno,
                    DataMatricula = d.DataMatricula,
                    DataSituacao = d.DataSituacao,
                    SituacaoCodigo = d.CodigoSituacaoMatricula,
                    NumeroChamada = d.NumeroAlunoChamada.HasValue ? d.NumeroAlunoChamada.Value : 0
                }).FirstOrDefault(d => d.DataMatricula <= periodoFim && d.DataSituacao.Date >= periodoInicio) ?? dadosAluno;

            bool validaMatricula = false;

            if (turmasComMatriculasValidas.Contains(turma.CodigoTurma))
            {
                if (alunoNaTurma != null)
                    validaMatricula = !MatriculaIgualDataConclusaoAlunoTurma(alunoNaTurma);

                var turmasCodigosFiltro = turmasCodigos.Distinct()
                    .ToArray();

                notasFechamentoAluno = fechamentoTurma != null && fechamentoTurma.PeriodoEscolarId.HasValue ?
                    await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigosFiltro, notasFrequenciaDto.AlunoCodigo, notasFrequenciaDto.Bimestre, dadosAluno.DataMatricula, !dadosAluno.EstaInativo() ? periodoFim : dadosAluno.DataSituacao, anoLetivo)) :
                    await mediator.Send(new ObterNotasFinaisBimestresAlunoQuery(turmasCodigosFiltro, notasFrequenciaDto.AlunoCodigo, dadosAluno.DataMatricula, !dadosAluno.EstaInativo() ? periodoFim : dadosAluno.DataSituacao, notasFrequenciaDto.Bimestre, validaMatricula));
            }

            var usuarioAtual = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if(!String.IsNullOrEmpty(turmaItinerarioPercurso))
                turmasCodigos.Add(turmaItinerarioPercurso);

            var disciplinasDaTurmaEol =
                (await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigos.ToArray(), usuarioAtual.PerfilAtual,
                    usuarioAtual.Login, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares, true))).ToList();

            if (notasFechamentoAluno.Any(x => x.Nota != null && turma.EhEJA() && turmaInicialEhEdFisica))
                ConverterNotaFechamentoAlunoNumerica(notasFechamentoAluno, turmaTipoNotaConceito);

            var disciplinasCodigo = disciplinasDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray();
            var disciplinasDaTurma = (await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(disciplinasCodigo, codigoTurma: turma.CodigoTurma))).ToList();
            var areasDoConhecimento = (await mediator.Send(new ObterAreasConhecimentoQuery(disciplinasDaTurmaEol))).ToList();
            var ordenacaoGrupoArea = (await mediator.Send(new ObterOrdenacaoAreasConhecimentoQuery(disciplinasDaTurma, areasDoConhecimento))).ToList();
            var retorno = new ConselhoClasseAlunoNotasConceitosRetornoDto();
            var gruposMatrizesNotas = new List<ConselhoClasseAlunoNotasConceitosDto>();
            
            var frequenciasAluno = turmasComMatriculasValidas.Contains(notasFrequenciaDto.CodigoTurma) && alunoNaTurma != null ?
                await ObterFrequenciaAlunoRefatorada(disciplinasDaTurmaEol, periodoEscolar, alunoNaTurma, tipoCalendario.Id, notasFrequenciaDto.Bimestre, tipoCalendario.AnoLetivo) :
                Enumerable.Empty<FrequenciaAluno>();

            var frequenciaAlunoRegenciaPai = new FrequenciaAluno();
            if (disciplinasDaTurmaEol.Any(x => x.Regencia))
                frequenciaAlunoRegenciaPai = frequenciasAluno.FirstOrDefault(f => f.DisciplinaId == disciplinasDaTurmaEol.FirstOrDefault(d => d.Regencia)?.CdComponenteCurricularPai.ToString());

            var registrosFrequencia = (turmasComMatriculasValidas.Contains(notasFrequenciaDto.CodigoTurma) && alunoNaTurma != null ?
                await mediator.Send(new ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery(notasFrequenciaDto.AlunoCodigo, turmasCodigos.ToArray(),
                    disciplinasCodigo.Select(d => d.ToString()).ToArray(), periodoEscolar?.Id)) :
                Enumerable.Empty<RegistroFrequenciaAlunoBimestreDto>()).ToList();

            var gruposMatrizes = disciplinasDaTurma
                .Where(c => c.GrupoMatrizNome != null && c.LancaNota)
                .OrderBy(d => d.GrupoMatrizId)
                .GroupBy(c => c.GrupoMatrizId)
                .ToList();

            var permiteEdicao = dadosAluno.DataMatricula.Date <= periodoFim && (dadosAluno.EstaAtivo() ||
                                dadosAluno.EstaInativo() && dadosAluno.DataSituacao.Date >= periodoInicio);

            var periodoMatricula = alunoNaTurma != null ? await mediator
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

                        var frequenciasAlunoParaTratar = frequenciasAluno.Where(a => (a.DisciplinaId == disciplina.Id.ToString() ||
                                                                                      a.DisciplinaId == disciplina.CodigoComponenteCurricular.ToString())
                                                         && (alunoNaTurma.DataMatricula <= dataFim
                                                         || MatriculaIgualDataConclusaoAlunoTurma(alunoNaTurma)));

                        frequenciasAlunoParaTratar = (periodoMatricula != null ? frequenciasAlunoParaTratar.Where(f => periodoMatricula.Bimestre <= f.Bimestre) : frequenciasAlunoParaTratar).ToList();

                        FrequenciaAluno frequenciaAluno;
                        var percentualFrequenciaPadrao = false;


                        if (frequenciasAlunoParaTratar == null || !frequenciasAlunoParaTratar.Any())
                            frequenciaAluno = new FrequenciaAluno() { DisciplinaId = disciplina.CodigoComponenteCurricular.ToString(), TurmaId = disciplinaEol.TurmaCodigo };
                        else if (frequenciasAlunoParaTratar.Count() == 1)
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
                                permiteEdicao, turmasCodigos.Distinct().ToArray(), frequenciaAlunoRegenciaPai);
                        }
                        else
                        {
                            var turmaPossuiRegistroFrequencia = VerificarSePossuiRegistroFrequencia(notasFrequenciaDto.AlunoCodigo, disciplinaEol.TurmaCodigo,
                                disciplina.CodigoComponenteCurricular, periodoEscolar, frequenciasAlunoParaTratar, registrosFrequencia);

                            conselhoClasseAlunoNotas.ComponentesCurriculares.Add(await ObterNotasFrequenciaComponente(disciplina.Nome,
                                disciplina.CodigoComponenteCurricular, frequenciaAluno, periodoEscolar, turma, notasConselhoClasseAluno,
                                notasFechamentoAluno, turmaPossuiRegistroFrequencia, disciplina.LancaNota, percentualFrequenciaPadrao,
                                permiteEdicao, turmasCodigos.Distinct().ToArray()));
                        }
                    }
                }

                if (conselhoClasseAlunoNotas.ComponentesCurriculares.Any())
                    conselhoClasseAlunoNotas.ComponentesCurriculares = conselhoClasseAlunoNotas.ComponentesCurriculares.OrderBy(c => c.Nome).ToList();

                if (conselhoClasseAlunoNotas.ComponenteRegencia != null)
                    conselhoClasseAlunoNotas.ComponenteRegencia.ComponentesCurriculares = conselhoClasseAlunoNotas.ComponenteRegencia.ComponentesCurriculares.OrderBy(c => c.Nome).ToList();

                gruposMatrizesNotas.Add(conselhoClasseAlunoNotas);
            }

            retorno.TemConselhoClasseAluno = notasFrequenciaDto.ConselhoClasseId > 0 && await VerificaSePossuiConselhoClasseAlunoAsync(notasFrequenciaDto.ConselhoClasseId, notasFrequenciaDto.AlunoCodigo);
            retorno.PodeEditarNota = permiteEdicao && await this.mediator.Send(new VerificaSePodeEditarNotaQuery(notasFrequenciaDto.AlunoCodigo, turma, periodoEscolar));
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

                var codigosTurmasRelacionadas = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turmaAluno.AnoLetivo, alunoCodigo, turmasCodigosParaConsulta));

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
            var dataReferencia = periodoFechamentoVigente?.PeriodoFechamentoFim ?? (await ObterPeriodoUltimoBimestre(turma)).PeriodoFim;
            return await mediator.Send(new ObterTipoNotaPorTurmaQuery(turma,dataReferencia));
        }
        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestrePorTurma(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await mediator.Send(new ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            if (periodoEscolarUltimoBimestre == null)
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FOI_ENCONTRADO_PERIODO_ULTIMO_BIMESTRE);

            return periodoEscolarUltimoBimestre;
        }

        private bool VerificarSePossuiRegistroFrequencia(string alunoCodigo, string turmaCodigo, long codigoComponenteCurricular, PeriodoEscolar periodoEscolar, IEnumerable<FrequenciaAluno> frequenciasAlunoParaTratar, IEnumerable<RegistroFrequenciaAlunoBimestreDto> registrosFrequencia)
        {
            return (frequenciasAlunoParaTratar != null && frequenciasAlunoParaTratar.Any()) ||
                   registrosFrequencia.Any(f => (periodoEscolar == null || f.Bimestre == periodoEscolar.Bimestre) &&
                                                f.CodigoAluno == alunoCodigo &&
                                                f.CodigoComponenteCurricular == codigoComponenteCurricular &&
                                                f.CodigoTurma == turmaCodigo);
        }
        
        private async Task<ConselhoClasseComponenteRegenciaFrequenciaDto> ObterNotasFrequenciaRegencia(long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, 
            Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno,
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota, bool visualizaNotas, string[] codigosTurma, FrequenciaAluno frequenciaAlunoRegenciaPai)
        {
            var componentesRegencia = await mediator.Send(new ObterComponentesRegenciaPorAnoQuery(turma.TipoTurno == 4 || turma.TipoTurno == 5 ? turma.AnoTurmaInteiro : 0));

            if (componentesRegencia == null || !componentesRegencia.Any())
                throw new NegocioException(MensagemNegocioComponentesCurriculares.NAO_FORAM_ENCONTRADOS_COMPONENTES_CURRICULARES_REGENCIA_INFORMADA);

            // Excessão de disciplina ED. Fisica para modalidade EJA
            if (turma.EhEJA())
                componentesRegencia = componentesRegencia.Where(a => a.CodigoComponenteCurricular != MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA);

            double percentualFrequencia;

            if(componentesRegencia != null && componentesRegencia.Any() && frequenciaAlunoRegenciaPai != null)
                percentualFrequencia = (frequenciaAlunoRegenciaPai.TotalAulas > 0 ? frequenciaAlunoRegenciaPai?.PercentualFrequencia ?? 0 : 0);
            else 
                percentualFrequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 0 : 0);

            // Cálculo de frequência particular do ano de 2020
            if (periodoEscolar == null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var conselhoClasseComponente = new ConselhoClasseComponenteRegenciaFrequenciaDto()
            {
                QuantidadeAulas = componentesRegencia != null && componentesRegencia.Any() ? frequenciaAlunoRegenciaPai?.TotalAulas ?? 0 : frequenciaAluno.TotalAulas,
                Faltas = componentesRegencia != null && componentesRegencia.Any() ? frequenciaAlunoRegenciaPai?.TotalAusencias ?? 0 : frequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = componentesRegencia != null && componentesRegencia.Any() ? frequenciaAlunoRegenciaPai?.TotalCompensacoes ?? 0  : frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = percentualFrequencia <= 0 ? "" : FrequenciaAluno.FormatarPercentual(percentualFrequencia)
            };

            foreach (var componenteRegencia in componentesRegencia)
                conselhoClasseComponente.ComponentesCurriculares.Add(await ObterNotasRegencia(componenteRegencia.Nome, componenteRegencia.CodigoComponenteCurricular, periodoEscolar, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota, turma.CodigoTurma, visualizaNotas,turma, codigosTurma));

            return conselhoClasseComponente;
        }
        
        private async Task<ConselhoClasseNotasComponenteRegenciaDto> ObterNotasRegencia(string componenteCurricularNome, long componenteCurricularCodigo, PeriodoEscolar periodoEscolar, 
            IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, 
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota, string codigoTurma, bool visualizaNotas,Turma turma, string[] codigosTurma)
        {
            return new ConselhoClasseNotasComponenteRegenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo,
                NotasFechamentos = ObterNotasFechamentoOuConselho(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = await ObterNotasPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota, visualizaNotas, codigoTurma, turma, codigosTurma)
            };
        }

        private async Task<NotaPosConselhoDto> ObterNotasPosConselho(long componenteCurricularCodigo, int? bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno,
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota, bool visualizaNota, string codigoTurma, Turma turma, string [] codigosTurma)
        {
            // Busca nota do conselho de classe consultado
            var notaComponente = notasConselhoClasseAluno.OrderByDescending(x => x.ConselhoClasseNotaId).FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo
            && c.TurmaCodigo.Equals(codigoTurma));
            var notaComponenteId = notaComponente?.ConselhoClasseNotaId;

            if (notaComponente == null || !notaComponente.NotaConceito.HasValue)
            {
                var notaComponenteFechamento = new NotaConceitoBimestreComponenteDto();

                if (turma.EhEJA() || turma.EhTurmaEnsinoMedio)
                {
                    notaComponenteFechamento =
                        notasFechamentoAluno.FirstOrDefault(t => codigosTurma.Contains(t.TurmaCodigo) && t.ComponenteCurricularCodigo == componenteCurricularCodigo && t.Bimestre == bimestre && t.ConselhoClasseNotaId > 0)
                        ?? notasFechamentoAluno.FirstOrDefault(t => codigosTurma.Contains(t.TurmaCodigo) && t.ComponenteCurricularCodigo == componenteCurricularCodigo && t.Bimestre == bimestre);
                }
                else
                {
                    notasFechamentoAluno = notasFechamentoAluno.Select(n => n.TurmaCodigo).Distinct().Count() > 1 ? notasFechamentoAluno.Where(n => n.TurmaCodigo == codigoTurma) : notasFechamentoAluno;

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
                NotaConceito = notaConceito,
            };
        }

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunoRefatorada(IEnumerable<DisciplinaDto> disciplinasDaTurma, PeriodoEscolar periodoEscolar, AlunoPorTurmaResposta alunoTurma,
            long tipoCalendarioId, int bimestre, int anoLetivo = 0)
        {
            var frequenciasAlunoRetorno = new List<FrequenciaAluno>();
            var disciplinasId = disciplinasDaTurma.Select(a => a.CodigoComponenteCurricular.ToString()).Distinct().ToList();
            var disciplinaIdRegenciaPai = disciplinasDaTurma.FirstOrDefault(d => d.Regencia && d.CdComponenteCurricularPai != 0)?.CdComponenteCurricularPai;
            if (disciplinaIdRegenciaPai.HasValue)
                disciplinasId.Add(disciplinaIdRegenciaPai.ToString());
            var disciplinas = disciplinasId.ToArray();

            var turmasCodigo = disciplinasDaTurma.Select(a => a.TurmaCodigo).Distinct().ToArray();

            int[] bimestres;
            if (periodoEscolar == null)
            {
                var periodosEscolaresTurma = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));

                if (periodosEscolaresTurma.Any())
                    bimestres = periodosEscolaresTurma.Select(a => a.Bimestre).ToArray();
                else throw new NegocioException(MensagemNegocioPeriodo.NAO_FORAM_ENCONTRADOS_PERIODOS_TURMA);
            }
            else bimestres = new int[] { bimestre };

            var frequenciasAluno = await mediator.Send(new ObterFrequenciaAlunoPorAlunoTipoTurmasDisciplinasTurmasBimestresPeriodoEscolarQuery(alunoTurma.CodigoAluno,
                                                         TipoFrequenciaAluno.PorDisciplina,
                                                         disciplinas,
                                                         turmasCodigo, bimestres));

            var aulasComponentesTurmas = await mediator
                .Send(new ObterTotalAulasTurmaEBimestreEComponenteCurricularQuery(turmasCodigo, tipoCalendarioId, disciplinas, bimestres, alunoTurma.DataMatricula, alunoTurma.Inativo ? alunoTurma.DataSituacao : null));

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
                        TotalAulas = 0,
                        Bimestre = aulaComponenteTurma.Bimestre,
                        PeriodoEscolarId = aulaComponenteTurma.PeriodoEscolarId
                    });
                }
            }

            // Cálculo específico para 2020
            if (periodoEscolar == null && anoLetivo.Equals(2020))
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
                var compensacoesDisciplina = compensacoes.Where(c => c.ComponenteCurricularId == disciplinaCodigo).FirstOrDefault();

                if (compensacoesDisciplina != null)
                    return compensacoesDisciplina.Compensacoes;
            }
            return 0;
        }
        
        private void ConverterNotaFechamentoAlunoNumerica(IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno,bool turmaTipoNotaConceito)
        {
            foreach (var notaFechamento in notasFechamentoAluno)
            {
                if (turmaTipoNotaConceito && MensagemNegocioComponentesCurriculares.COMPONENTE_CURRICULAR_CODIGO_ED_FISICA.Equals(notaFechamento.ComponenteCurricularCodigo))
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
        }
        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestre(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await mediator.Send(new ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre));
            if (periodoEscolarUltimoBimestre == null)
                throw new NegocioException(MensagemNegocioPeriodo.NAO_FOI_ENCONTRADO_PERIODO_ULTIMO_BIMESTRE);

            return periodoEscolarUltimoBimestre;
        }
        private bool MatriculaIgualDataConclusaoAlunoTurma(AlunoPorTurmaResposta alunoNaTurma)
        {
            return alunoNaTurma.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Concluido && alunoNaTurma.DataMatricula.Date == alunoNaTurma.DataSituacao.Date;
        }

        private async Task<ConselhoClasseComponenteFrequenciaDto> ObterNotasFrequenciaComponente(string componenteCurricularNome, long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar,
            Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool turmaPossuiRegistroFrequencia, bool componenteLancaNota, 
            bool percentualFrequenciaPadrao, bool visualizaNota, string[] codigosTurma)
        {
            var totalAulas = Enumerable.Empty<TotalAulasPorAlunoTurmaDto>();
            var componentePermiteFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(componenteCurricularCodigo));
            var bimestre = periodoEscolar?.Bimestre == null ? 0 : periodoEscolar.Bimestre;
            var percentualFrequencia = double.MinValue;

            if (turmaPossuiRegistroFrequencia && frequenciaAluno != null)
                percentualFrequencia = frequenciaAluno.PercentualFrequencia;

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
                Frequencia = percentualFrequencia < 0 || ((frequenciaAluno?.TotalAulas ?? 0) == 0 && (frequenciaAluno?.TotalAusencias ?? 0) == 0) ? null : FrequenciaAluno.FormatarPercentual(percentualFrequencia),
                NotasFechamentos = ObterNotasFechamentoOuConselho(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = await ObterNotasPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota, visualizaNota, turma.CodigoTurma, turma, codigosTurma),
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

        private bool ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(IEnumerable<Turma> turmasAluno, Turma turmaFiltro)
            => turmasAluno.Select(t => t.TipoTurma).Distinct().Count() == 1 && turmaFiltro.ModalidadeCodigo == Modalidade.Medio && (turmaFiltro.AnoLetivo < 2021 || turmaFiltro.Ano == PRIMEIRO_ANO_EM);

        private async Task<bool> VerificaSePossuiConselhoClasseAlunoAsync(long conselhoClasseId, string alunoCodigo)
        {
            var conselhoClasseAlunoId = await mediator.Send(new ObterConselhoClasseAlunoIdQuery(conselhoClasseId, alunoCodigo));
            return conselhoClasseAlunoId > 0;
        }

    }
}