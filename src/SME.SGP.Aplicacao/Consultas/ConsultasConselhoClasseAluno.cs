﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasseAluno : IConsultasConselhoClasseAluno
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasConselhoClasseNota consultasConselhoClasseNota;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoConselhoClasse servicoConselhoClasse;
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasConselhoClasseAluno(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                            IRepositorioTurma repositorioTurma,
                                            IConsultasDisciplina consultasDisciplina,
                                            IRepositorioTipoCalendario repositorioTipoCalendario,
                                            IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                            IConsultasConselhoClasseNota consultasConselhoClasseNota,
                                            IServicoEol servicoEOL,
                                            IServicoUsuario servicoUsuario,
                                            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                            IConsultasFrequencia consultasFrequencia,
                                            IServicoConselhoClasse servicoConselhoClasse,
                                            IMediator mediator)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasConselhoClasseNota = consultasConselhoClasseNota ?? throw new ArgumentNullException(nameof(consultasConselhoClasseNota));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.servicoConselhoClasse = servicoConselhoClasse ?? throw new ArgumentNullException(nameof(servicoConselhoClasse));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> ExisteConselhoClasseUltimoBimestreAsync(Turma turma, string alunoCodigo)
        {
            var periodoEscolar = await ObterPeriodoUltimoBimestre(turma);

            var conselhoClasseUltimoBimestre = await repositorioConselhoClasseAluno.ObterPorPeriodoAsync(alunoCodigo, turma.Id, periodoEscolar.Id);
            if (conselhoClasseUltimoBimestre == null)
                return false;

            return await servicoConselhoClasse.VerificaNotasTodosComponentesCurriculares(alunoCodigo, turma, periodoEscolar.Id);
        }

        public async Task<IEnumerable<ConselhoDeClasseGrupoMatrizDto>> ObterListagemDeSinteses(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre)
        {
            var retorno = new List<ConselhoDeClasseGrupoMatrizDto>();
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, turma.EhAnoAnterior()));

            if (fechamentoTurma != null)
                turma = fechamentoTurma?.Turma;
            else
            {
                if (!turma.EhAnoAnterior())
                    throw new NegocioException("Não existe fechamento para a turma");
            }

            if (turma.AnoLetivo != 2020 && turma.AnoLetivo == DateTime.Now.Year && bimestre == 0 && !await ExisteConselhoClasseUltimoBimestreAsync(turma, alunoCodigo))
                throw new NegocioException("Aluno não possui conselho de classe do último bimestre");



            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurma(turma.CodigoTurma);
            if (disciplinas == null || !disciplinas.Any())
                return null;

            var gruposMatrizes = disciplinas.Where(x => !x.LancaNota && x.GrupoMatriz != null)
                                            .GroupBy(c => new { Id = c.GrupoMatriz?.Id, Nome = c.GrupoMatriz?.Nome });

            var frequenciaAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaBimestresAsync(alunoCodigo, bimestre, turma.CodigoTurma);

            foreach (var grupoDisiplinasMatriz in gruposMatrizes.OrderBy(k => k.Key.Nome))
            {
                var grupoMatriz = new ConselhoDeClasseGrupoMatrizDto()
                {
                    Id = grupoDisiplinasMatriz.Key.Id ?? 0,
                    Titulo = grupoDisiplinasMatriz.Key.Nome ?? "",
                    ComponenteSinteses = new List<ConselhoDeClasseComponenteSinteseDto>()
                };

                foreach (var componenteCurricular in grupoDisiplinasMatriz.Where(x => x.LancaNota))
                {
                    var componenteCurricularDto = await MapearDto(frequenciaAluno, componenteCurricular, bimestre);
                    grupoMatriz.ComponenteSinteses.Add(componenteCurricularDto);
                }

                retorno.Add(grupoMatriz);
            }

            return retorno;
        }

        public async Task<ConselhoClasseAlunoNotasConceitosRetornoDto> ObterNotasFrequencia(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, turma.AnoLetivo != DateTime.Now.Year));
            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma != null) turma = fechamentoTurma?.Turma;
            else
            {
                if (bimestre > 0)
                    periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, bimestre));
            }

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (tipoCalendario == null) throw new NegocioException("Tipo de calendário não encontrado");

            string[] turmasCodigos;
            long[] conselhosClassesIds;

            if (turma.DeveVerificarRegraRegulares())
            {
                List<TipoTurma> turmasCodigosParaConsulta = new List<TipoTurma>() { turma.TipoTurma };
                turmasCodigosParaConsulta.AddRange(turma.ObterTiposRegularesDiferentes());
                turmasCodigos = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(turma.AnoLetivo, alunoCodigo, turmasCodigosParaConsulta));
                conselhosClassesIds = await mediator.Send(new ObterConselhoClasseIdsPorTurmaEPeriodoQuery(turmasCodigos, periodoEscolar?.Id));
            }
            else
            {
                turmasCodigos = new string[1] { turma.CodigoTurma };
                conselhosClassesIds = new long[1] { conselhoClasseId };
            }

            var notasConselhoClasseAluno = new List<NotaConceitoBimestreComponenteDto>();

            if (conselhosClassesIds != null)
            {
                foreach (var conselhosClassesId in conselhosClassesIds)
                {
                    var notasParaAdicionar = await consultasConselhoClasseNota.ObterNotasAlunoAsync(conselhosClassesId, alunoCodigo);
                    notasConselhoClasseAluno.AddRange(notasParaAdicionar);
                }
            }

            List<Turma> turmas;

            if (turmasCodigos.Length > 0)
                turmas = (await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos))).ToList();
            else turmas = new List<Turma>() { turma };

            //Verificar as notas finais
            var notasFechamentoAluno = fechamentoTurma != null && fechamentoTurma.PeriodoEscolarId.HasValue ?
               await mediator.Send(new ObterNotasFechamentosPorTurmasCodigosBimestreQuery(turmasCodigos, alunoCodigo, bimestre)) :
               await consultasConselhoClasseNota.ObterNotasFinaisBimestresAlunoAsync(alunoCodigo, turmasCodigos, bimestre);

            Usuario usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var disciplinasDaTurmaEol = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigos, usuarioAtual.PerfilAtual, usuarioAtual.Login, turma.EnsinoEspecial, turma.TurnoParaComponentesCurriculares));

            var disciplinasCodigo = disciplinasDaTurmaEol.Select(x => x.CodigoComponenteCurricular).Distinct().ToArray();

            var disciplinasDaTurma = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(disciplinasCodigo));

            var retorno = new ConselhoClasseAlunoNotasConceitosRetornoDto();

            var gruposMatrizesNotas = new List<ConselhoClasseAlunoNotasConceitosDto>();

            var frequenciasAluno = await ObterFrequenciaAlunoRefatorada(disciplinasDaTurmaEol, periodoEscolar, alunoCodigo, tipoCalendario.Id, bimestre);

            var registrosFrequencia = await mediator.Send(new ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery(alunoCodigo, turmasCodigos, disciplinasCodigo.Select(d => d.ToString()).ToArray(), periodoEscolar?.Id));

            var gruposMatrizes = disciplinasDaTurma.Where(c => c.RegistraFrequencia && c.GrupoMatrizNome != null).OrderBy(d => d.GrupoMatrizId).GroupBy(c => c.GrupoMatrizNome).ToList();

            foreach (var grupoDisiplinasMatriz in gruposMatrizes)
            {
                var conselhoClasseAlunoNotas = new ConselhoClasseAlunoNotasConceitosDto();
                conselhoClasseAlunoNotas.GrupoMatriz = grupoDisiplinasMatriz.Key;

                foreach (var disciplina in grupoDisiplinasMatriz.OrderBy(g => g.Nome))
                {
                    var disciplinaEol = disciplinasDaTurmaEol.FirstOrDefault(d => d.CodigoComponenteCurricular == disciplina.Id);

                    var frequenciasAlunoParaTratar = frequenciasAluno.Where(a => a.DisciplinaId == disciplina.Id.ToString());
                    FrequenciaAluno frequenciaAluno;

                    if (frequenciasAlunoParaTratar == null || !frequenciasAlunoParaTratar.Any())
                    {
                        frequenciaAluno = new FrequenciaAluno() { DisciplinaId = disciplina.Id.ToString(), TurmaId = disciplinaEol.TurmaCodigo };
                    }
                    else if (frequenciasAlunoParaTratar.Count() == 1)
                    {
                        frequenciaAluno = frequenciasAlunoParaTratar.FirstOrDefault();
                    }
                    else
                    {
                        frequenciaAluno = new FrequenciaAluno()
                        {
                            DisciplinaId = disciplina.CodigoComponenteCurricular.ToString(),
                            CodigoAluno = alunoCodigo
                        };

                        frequenciaAluno.TotalAulas = frequenciasAlunoParaTratar.Sum(a => a.TotalAulas);
                        frequenciaAluno.TotalAusencias = frequenciasAlunoParaTratar.Sum(a => a.TotalAusencias);
                        frequenciaAluno.TotalCompensacoes = frequenciasAlunoParaTratar.Sum(a => a.TotalCompensacoes);
                        frequenciasAlunoParaTratar.ToList().ForEach(f => frequenciaAluno.AdicionarFrequenciaBimestre(f.Bimestre, tipoCalendario.AnoLetivo.Equals(2020) && f.TotalAulas.Equals(0) ? 100 : f.PercentualFrequencia));
                    }

                    if (disciplina.Regencia)
                    {
                        conselhoClasseAlunoNotas.ComponenteRegencia = await ObterNotasFrequenciaRegencia(disciplina.CodigoComponenteCurricular,
                                                                                                         frequenciaAluno,
                                                                                                         periodoEscolar,
                                                                                                         turma,
                                                                                                         notasConselhoClasseAluno,
                                                                                                         notasFechamentoAluno,
                                                                                                         disciplina.LancaNota);
                    }
                    else
                    {
                        var turmaPossuiRegistroFrequencia = VerificarSePossuiRegistroFrequencia(alunoCodigo, disciplinaEol.TurmaCodigo, disciplina.CodigoComponenteCurricular,
                                                                                                periodoEscolar, frequenciasAlunoParaTratar, registrosFrequencia);

                        conselhoClasseAlunoNotas.ComponentesCurriculares.Add(ObterNotasFrequenciaComponente(disciplina.Nome,
                                                                                                            disciplina.CodigoComponenteCurricular,
                                                                                                            frequenciaAluno,
                                                                                                            periodoEscolar,
                                                                                                            turma,
                                                                                                            notasConselhoClasseAluno,
                                                                                                            notasFechamentoAluno,
                                                                                                            turmaPossuiRegistroFrequencia,
                                                                                                            disciplina.LancaNota));
                    }
                }

                gruposMatrizesNotas.Add(conselhoClasseAlunoNotas);
            }

            retorno.PodeEditarNota = await VerificaSePodeEditarNota(alunoCodigo, turma, periodoEscolar);
            retorno.NotasConceitos = gruposMatrizesNotas;

            return retorno;
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
            var turmaFechamento = await servicoEOL.ObterAlunosPorTurma(turma.CodigoTurma, turma.AnoLetivo);

            if (turmaFechamento == null || !turmaFechamento.Any())
                throw new NegocioException($"Não foi possível obter os dados da turma {turma.CodigoTurma}");

            var aluno = turmaFechamento.FirstOrDefault(a => a.CodigoAluno == alunoCodigo);
            if (aluno == null)
                throw new NegocioException($"Não foi possível obter os dados do aluno {alunoCodigo}");
            return aluno.PodeEditarNotaConceitoNoPeriodo(periodoEscolar);
        }

        public async Task<ParecerConclusivoDto> ObterParecerConclusivo(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));
            if (turma == null)
                throw new NegocioException("Não foi possível obter os dados da turma");

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(fechamentoTurmaId, alunoCodigo, turma.AnoLetivo != DateTime.Now.Year));

            if (fechamentoTurma == null)
            {
                if (!turma.EhAnoAnterior())
                    throw new NegocioException("Não existe fechamento para a turma");
            }
            else
            {
                turma = fechamentoTurma.Turma;
            }

            if (turma.AnoLetivo != 2020 && !turma.EhAnoAnterior() && !await ExisteConselhoClasseUltimoBimestreAsync(turma, alunoCodigo))
                throw new NegocioException("Aluno não possui conselho de classe do último bimestre");

            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
            if (!turma.EhAnoAnterior() && (conselhoClasseAluno == null || !conselhoClasseAluno.ConselhoClasseParecerId.HasValue))
                return await servicoConselhoClasse.GerarParecerConclusivoAlunoAsync(conselhoClasseId, fechamentoTurmaId, alunoCodigo);

            return new ParecerConclusivoDto()
            {
                Id = conselhoClasseAluno?.ConselhoClasseParecerId != null ? conselhoClasseAluno.ConselhoClasseParecerId.Value : 0,
                Nome = conselhoClasseAluno?.ConselhoClasseParecer?.Nome
            };
        }

        public async Task<ConselhoClasseAluno> ObterPorConselhoClasseAsync(long conselhoClasseId, string alunoCodigo)
            => await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);

        private static DisciplinaDto MapeaderDisciplinasDto(DisciplinaResposta componenteCurricular)
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

        private static ConselhoDeClasseComponenteSinteseDto MapearConselhoDeClasseComponenteSinteseDto(DisciplinaResposta componenteCurricular, IEnumerable<FrequenciaAluno> frequenciaDisciplina, double percentualFrequencia, SinteseDto parecerFinal)
        {
            return new ConselhoDeClasseComponenteSinteseDto
            {
                Codigo = componenteCurricular.CodigoComponenteCurricular,
                Nome = componenteCurricular.Nome,
                TotalFaltas = frequenciaDisciplina.Sum(x => x.TotalAusencias),
                PercentualFrequencia = percentualFrequencia,
                ParecerFinal = parecerFinal?.Valor ?? string.Empty,
                ParecerFinalId = (int)(parecerFinal?.Id ?? default)
            };
        }

        private static IEnumerable<FrequenciaAluno> ObterFrequenciaPorDisciplina(IEnumerable<FrequenciaAluno> frequenciaAluno, DisciplinaResposta componenteCurricular)
        {
            return frequenciaAluno.Where(x => x.DisciplinaId == componenteCurricular.CodigoComponenteCurricular.ToString());
        }

        private static double ObterPercentualDeFrequencia(IEnumerable<FrequenciaAluno> frequenciaDisciplina)
        {
            return frequenciaDisciplina.Any() ? frequenciaDisciplina.Sum(x => x.PercentualFrequencia) / frequenciaDisciplina.Count() : 100;
        }

        private async Task<ConselhoDeClasseComponenteSinteseDto> MapearDto(IEnumerable<FrequenciaAluno> frequenciaAluno, DisciplinaResposta componenteCurricular, int bimestre)
        {
            var frequenciaDisciplina = ObterFrequenciaPorDisciplina(frequenciaAluno, componenteCurricular);

            var percentualFrequencia = ObterPercentualDeFrequencia(frequenciaDisciplina);

            var dto = MapeaderDisciplinasDto(componenteCurricular);

            var parecerFinal = bimestre == 0 ? await consultasFrequencia.ObterSinteseAluno(percentualFrequencia, dto) : null;

            var componenteSinteseAdicionar = MapearConselhoDeClasseComponenteSinteseDto(componenteCurricular, frequenciaDisciplina, percentualFrequencia, parecerFinal);
            return componenteSinteseAdicionar;
        }

        private async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunoRefatorada(IEnumerable<DisciplinaDto> disciplinasDaTurma, PeriodoEscolar periodoEscolar, string alunoCodigo,
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
                {
                    bimestres = periodosEscolaresTurma.Select(a => a.Bimestre).ToArray();
                }
                else throw new NegocioException("Não foi possível localizar os períodos escolares da turma");
            }
            else bimestres = new int[] { bimestre };

            var frequenciasAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoTurmasDisciplinasDataAsync(alunoCodigo,
                                                                                  TipoFrequenciaAluno.PorDisciplina,
                                                                                  disciplinasId,
                                                                                  turmasCodigo, bimestres);

            var aulasComponentesTurmas = await mediator.Send(new ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery(turmasCodigo, tipoCalendarioId, disciplinasId, bimestres));

            if (frequenciasAluno != null && frequenciasAluno.Any())
                frequenciasAlunoRetorno.AddRange(frequenciasAluno);

            foreach (var aulaComponenteTurma in aulasComponentesTurmas)
            {
                if (!frequenciasAlunoRetorno.Any(a => a.TurmaId == aulaComponenteTurma.TurmaCodigo && a.DisciplinaId == aulaComponenteTurma.ComponenteCurricularCodigo && a.Bimestre == aulaComponenteTurma.Bimestre))
                {
                    frequenciasAlunoRetorno.Add(new FrequenciaAluno()
                    {
                        CodigoAluno = alunoCodigo,
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

        private NotaBimestreDto ObterNotaFinalComponentePeriodo(long codigoComponenteCurricular, int bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
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

        private NotaPosConselhoDto ObterNotaPosConselho(long componenteCurricularCodigo, int? bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota)
        {
            // Busca nota do conselho de classe consultado
            var notaComponente = notasConselhoClasseAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo);
            if (notaComponente == null)
            {
                // Sugere nota final do fechamento
                var notaComponenteComConselhoNota = notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == bimestre && c.ConselhoClasseNotaId > 0);
                if (notaComponenteComConselhoNota != null) notaComponente = notaComponenteComConselhoNota;
                else
                    notaComponente = notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == bimestre);
            }

            return new NotaPosConselhoDto()
            {
                Id = notaComponente?.Id,
                Nota = notaComponente?.NotaConceito,
                PodeEditar = componenteLancaNota
            };
        }

        private List<NotaBimestreDto> ObterNotasComponente(long componenteCurricularCodigo, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var notasFinais = new List<NotaBimestreDto>();

            if (periodoEscolar != null)
                notasFinais.Add(ObterNotaFinalComponentePeriodo(componenteCurricularCodigo, periodoEscolar.Bimestre, notasFechamentoAluno));
            else
                notasFinais.AddRange(ObterNotasFinaisComponentePeriodos(componenteCurricularCodigo, notasFechamentoAluno));

            return notasFinais;
        }

        private IEnumerable<NotaBimestreDto> ObterNotasFinaisComponentePeriodos(long codigoComponenteCurricular, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
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

        private ConselhoClasseComponenteFrequenciaDto ObterNotasFrequenciaComponente(string componenteCurricularNome, long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool turmaPossuiRegistroFrequencia, bool componenteLancaNota)
        {
            var percentualFrequencia = 0.0;

            if (turmaPossuiRegistroFrequencia)
                percentualFrequencia = (int)Math.Round(frequenciaAluno != null ? frequenciaAluno.PercentualFrequencia : 100);

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
                Frequencia = percentualFrequencia < 0 ? null : percentualFrequencia.ToString(),
                NotasFechamentos = ObterNotasComponente(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = ObterNotaPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota)
            };

            return conselhoClasseComponente;
        }
        private async Task<ConselhoClasseComponenteRegenciaFrequenciaDto> ObterNotasFrequenciaRegencia(long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno,
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota)
        {
            var componentesRegencia = await consultasDisciplina.ObterComponentesRegencia(turma, componenteCurricularCodigo);

            if (componentesRegencia == null || !componentesRegencia.Any())
                throw new NegocioException("Não foram encontrados componentes curriculares para a regência informada.");

            var percentualFrequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 100 : 100);

            // Cálculo de frequência particular do ano de 2020
            if (periodoEscolar == null && turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno.PercentualFrequenciaFinal;

            var conselhoClasseComponente = new ConselhoClasseComponenteRegenciaFrequenciaDto()
            {
                QuantidadeAulas = frequenciaAluno.TotalAulas,
                Faltas = frequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = percentualFrequencia
            };

            foreach (var componenteRegencia in componentesRegencia)
                conselhoClasseComponente.ComponentesCurriculares.Add(ObterNotasRegencia(componenteRegencia.Nome, componenteRegencia.CodigoComponenteCurricular, periodoEscolar, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota));

            return conselhoClasseComponente;
        }

        private ConselhoClasseNotasComponenteRegenciaDto ObterNotasRegencia(string componenteCurricularNome, long componenteCurricularCodigo, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, bool componenteLancaNota)
        {
            return new ConselhoClasseNotasComponenteRegenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo,
                NotasFechamentos = ObterNotasComponente(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = ObterNotaPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno, componenteLancaNota)
            };
        }

        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestre(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await consultasPeriodoEscolar.ObterUltimoPeriodoAsync(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (periodoEscolarUltimoBimestre == null)
                throw new NegocioException("Não foi possível localizar o período escolar do ultimo bimestre da turma");

            return periodoEscolarUltimoBimestre;
        }
    }
}