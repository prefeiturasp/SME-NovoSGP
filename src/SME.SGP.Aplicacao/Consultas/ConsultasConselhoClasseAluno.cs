using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
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
        private readonly IConsultasAulaPrevista consultasAulaPrevista;
        private readonly IConsultasConselhoClasseNota consultasConselhoClasseNota;
        private readonly IConsultasFechamentoNota consultasFechamentoNota;
        private readonly IConsultasFechamentoTurma consultasFechamentoTurma;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IServicoConselhoClasse servicoConselhoClasse;
        private readonly IMediator mediator;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasConselhoClasseAluno(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                            IRepositorioTurma repositorioTurma,
                                            IRepositorioTipoCalendario repositorioTipoCalendario,
                                            IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                            IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                            IConsultasTipoCalendario consultasTipoCalendario,
                                            IConsultasFechamentoTurma consultasFechamentoTurma,
                                            IConsultasAulaPrevista consultasAulaPrevista,
                                            IConsultasConselhoClasseNota consultasConselhoClasseNota,
                                            IConsultasFechamentoNota consultasFechamentoNota,
                                            IServicoEol servicoEOL,
                                            IServicoUsuario servicoUsuario,
                                            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                            IConsultasFrequencia consultasFrequencia,
                                            IServicoConselhoClasse servicoConselhoClasse,
                                            IMediator mediator)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
            this.consultasAulaPrevista = consultasAulaPrevista ?? throw new ArgumentNullException(nameof(consultasAulaPrevista));
            this.consultasConselhoClasseNota = consultasConselhoClasseNota ?? throw new ArgumentNullException(nameof(consultasConselhoClasseNota));
            this.consultasFechamentoNota = consultasFechamentoNota ?? throw new ArgumentNullException(nameof(consultasFechamentoNota));
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

            var fechamentoTurma = await consultasFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);

            var turma = fechamentoTurma?.Turma;

            if (fechamentoTurma == null)
            {
                turma = await repositorioTurma.ObterPorCodigo(codigoTurma);
                if(turma == null)
                    throw new NegocioException("Turma não encontrada");
                if (turma.AnoLetivo == DateTime.Today.Year)
                    throw new NegocioException("Não existe fechamento para a turma");
            }

            if (turma.AnoLetivo == DateTime.Now.Year && bimestre == 0 && !await ExisteConselhoClasseUltimoBimestreAsync(turma, alunoCodigo))
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

                foreach (var componenteCurricular in grupoDisiplinasMatriz)
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
            var fechamentoTurma = await consultasFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);
            var turma = fechamentoTurma?.Turma;
            var periodoEscolar = fechamentoTurma?.PeriodoEscolar;

            if (fechamentoTurma == null)
            {
                turma = await repositorioTurma.ObterPorCodigo(codigoTurma);
                if(turma == null) throw new NegocioException("Turma não localizada");

                var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
                if (tipoCalendario == null) throw new NegocioException("Tipo de calendáro não encontrado");

                periodoEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendarioEBimestreAsync(tipoCalendario.Id, bimestre);

                if(turma.AnoLetivo == DateTime.Today.Year) throw new NegocioException("Fechamento da Turma não encontrado");
            }

            var turmaCodigo = turma.CodigoTurma;

            var notasConselhoClasseAluno = await consultasConselhoClasseNota.ObterNotasAlunoAsync(conselhoClasseId, alunoCodigo);
            var notasFechamentoAluno = fechamentoTurma != null && fechamentoTurma.PeriodoEscolarId.HasValue ?
                await consultasFechamentoNota.ObterNotasAlunoBimestreAsync(fechamentoTurmaId, alunoCodigo) :
                await consultasConselhoClasseNota.ObterNotasFinaisBimestresAlunoAsync(alunoCodigo, turmaCodigo);


             var disciplinasComRegencia = (await mediator.Send(new ObterComponentesCurricularesPorUeAnosModalidadeQuery(new long[] { long.Parse(turma.CodigoTurma) }, turma.AnoLetivo,
                 turma.ModalidadeCodigo, new string[] { turma.Ano } ))).ToList();
            
            var disciplinasDaTurma = await servicoEOL.ObterDisciplinasPorCodigoTurma(turmaCodigo);
            

            //if (disciplinas == null)
            //    throw new NegocioException("disciplinas da turma não localizadas no eol");

            var retorno = new ConselhoClasseAlunoNotasConceitosRetornoDto();

            var gruposMatrizesNotas = new List<ConselhoClasseAlunoNotasConceitosDto>();
            // Retornar componentes que lançam nota
            var gruposMatrizes = disciplinasDaTurma.Where(c => c.LancaNota && c.GrupoMatriz != null).GroupBy(c => c.GrupoMatriz?.Nome).ToList();
            foreach (var grupoDisiplinasMatriz in gruposMatrizes.OrderBy(k => k.Key))
            {
                var conselhoClasseAlunoNotas = new ConselhoClasseAlunoNotasConceitosDto();
                conselhoClasseAlunoNotas.GrupoMatriz = grupoDisiplinasMatriz.Key;

                foreach (var disciplina in grupoDisiplinasMatriz)
                {
                    // Carrega Frequencia Aluno
                    var frequenciaAluno = await ObterFrequenciaAluno(turma,
                                                                     periodoEscolar,
                                                                     disciplina.CodigoComponenteCurricular,
                                                                     alunoCodigo);

                    if (disciplina.Regencia)
                    {
                        conselhoClasseAlunoNotas.ComponenteRegencia = await ObterNotasFrequenciaRegencia(disciplina.Nome,
                                                                                                         disciplina.CodigoComponenteCurricular,
                                                                                                         frequenciaAluno,
                                                                                                         periodoEscolar,
                                                                                                         turma,
                                                                                                         notasConselhoClasseAluno,
                                                                                                         notasFechamentoAluno,
                                                                                                         disciplinasComRegencia.Where(a => a.Regencia).ToList());
                    }else
                        conselhoClasseAlunoNotas.ComponentesCurriculares.Add(ObterNotasFrequenciaComponente(disciplina.Nome,
                                                                                                            disciplina.CodigoComponenteCurricular,
                                                                                                            frequenciaAluno,
                                                                                                            periodoEscolar,
                                                                                                            turma,
                                                                                                            notasConselhoClasseAluno,
                                                                                                            notasFechamentoAluno));
                }
                gruposMatrizesNotas.Add(conselhoClasseAlunoNotas);
            }

            retorno.PodeEditarNota = await VerificaSePodeEditarNota(alunoCodigo, turma, periodoEscolar);
            retorno.NotasConceitos = gruposMatrizesNotas;

            return retorno;
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
            var fechamentoTurma = await consultasFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);
            var turma = fechamentoTurma?.Turma;
            var ehAnoAtual = false;

            if (fechamentoTurma == null)
            {
                turma = await repositorioTurma.ObterPorCodigo(codigoTurma);
                if(turma == null) throw new NegocioException("Turma não encontrada");

                if(ehAnoAtual)
                throw new NegocioException("Não existe fechamento para a turma");
            }

            ehAnoAtual = turma.AnoLetivo == DateTime.Now.Year;

            if (ehAnoAtual && !await ExisteConselhoClasseUltimoBimestreAsync(turma, alunoCodigo))
                throw new NegocioException("Aluno não possui conselho de classe do último bimestre");

            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
            if (ehAnoAtual && (conselhoClasseAluno == null || !conselhoClasseAluno.ConselhoClasseParecerId.HasValue))
                return await servicoConselhoClasse.GerarParecerConclusivoAlunoAsync(conselhoClasseId, fechamentoTurmaId, alunoCodigo);

            return new ParecerConclusivoDto()
            {
                Id = conselhoClasseAluno?.ConselhoClasseParecerId != null  ? conselhoClasseAluno.ConselhoClasseParecerId.Value: 0,
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

        private async Task<FrequenciaAluno> ObterFrequenciaAluno(Turma turma, PeriodoEscolar periodoEscolar, long componenteCurricularCodigo, string alunoCodigo)
        {
            var frequenciaAluno = new FrequenciaAluno();
            if (periodoEscolar != null)
            {
                // Frequencia do bimestre
                frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(alunoCodigo,
                                                                               periodoEscolar.PeriodoFim,
                                                                               TipoFrequenciaAluno.PorDisciplina,
                                                                               componenteCurricularCodigo.ToString());
                if (frequenciaAluno != null)
                    return frequenciaAluno;

                return new FrequenciaAluno()
                {
                    TotalAulas = await consultasAulaPrevista.ObterAulasDadas(turma,
                                                                             componenteCurricularCodigo.ToString(),
                                                                             periodoEscolar.Bimestre)
                };
            }
            else
            {
                // Frequencia Anual - totaliza todos os bimestres
                var tipoCalendario = await consultasTipoCalendario.ObterPorTurma(turma);
                if (tipoCalendario == null)
                    throw new NegocioException("Tipo de calendário não localizado para a turma");

                var periodosEscolaresTurma = await consultasPeriodoEscolar.ObterPeriodosEscolares(tipoCalendario.Id);
                foreach (var periodoEscolarTurma in periodosEscolaresTurma)
                {
                    var frequenciaAlunoPeriodo = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoBimestreAsync(alunoCodigo,
                                                                                                             periodoEscolarTurma.Bimestre,
                                                                                                             TipoFrequenciaAluno.PorDisciplina,
                                                                                                             componenteCurricularCodigo.ToString());
                    if (frequenciaAlunoPeriodo != null)
                    {
                        frequenciaAluno.TotalAulas += frequenciaAlunoPeriodo.TotalAulas;
                        frequenciaAluno.TotalAusencias += frequenciaAlunoPeriodo.TotalAusencias;
                        frequenciaAluno.TotalCompensacoes += frequenciaAlunoPeriodo.TotalCompensacoes;
                    }
                    else
                        // Se não tem ausencia não vai ter registro de frequencia então soma apenas aulas do bimestre
                        frequenciaAluno.TotalAulas += await consultasAulaPrevista.ObterAulasDadas(turma,
                                                                                                  componenteCurricularCodigo.ToString(),
                                                                                                  periodoEscolarTurma.Bimestre);
                }

                return frequenciaAluno;
            }
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

        private NotaPosConselhoDto ObterNotaPosConselho(long componenteCurricularCodigo, int? bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            // Busca nota do conselho de classe consultado
            var notaComponente = notasConselhoClasseAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo);
            if (notaComponente == null)
                // Sugere nota final do fechamento
                notaComponente = notasFechamentoAluno.FirstOrDefault(c => c.ComponenteCurricularCodigo == componenteCurricularCodigo && c.Bimestre == bimestre);

            return new NotaPosConselhoDto()
            {
                Id = notaComponente?.Id,
                Nota = notaComponente?.NotaConceito
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

        private ConselhoClasseComponenteFrequenciaDto ObterNotasFrequenciaComponente(string componenteCurricularNome, long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var conselhoClasseComponente = new ConselhoClasseComponenteFrequenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo,
                QuantidadeAulas = frequenciaAluno.TotalAulas,
                Faltas = frequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 100 : 100),
                NotasFechamentos = ObterNotasComponente(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = ObterNotaPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno)
            };

            return conselhoClasseComponente;
        }

        private async Task<ConselhoClasseComponenteRegenciaFrequenciaDto> ObterNotasFrequenciaRegencia(string componenteCurricularNome, long componenteCurricularCodigo, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, 
            IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno, IEnumerable<ComponenteCurricularEol> componentesCurricularesEol)
        {
            var conselhoClasseComponente = new ConselhoClasseComponenteRegenciaFrequenciaDto()
            {
                QuantidadeAulas = frequenciaAluno.TotalAulas,
                Faltas = frequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 100 : 100)
            };

            //var componentesRegencia = await consultasDisciplina.ObterComponentesRegencia(turma, componenteCurricularCodigo);
            foreach (var componenteRegencia in componentesCurricularesEol)
            {
                conselhoClasseComponente.ComponentesCurriculares.Add(ObterNotasRegencia(componenteRegencia.Descricao, componenteRegencia.Codigo, periodoEscolar, notasConselhoClasseAluno, notasFechamentoAluno));
            }

            return conselhoClasseComponente;
        }

        private ConselhoClasseNotasComponenteRegenciaDto ObterNotasRegencia(string componenteCurricularNome, long componenteCurricularCodigo, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            return new ConselhoClasseNotasComponenteRegenciaDto()
            {
                Nome = componenteCurricularNome,
                CodigoComponenteCurricular = componenteCurricularCodigo,
                NotasFechamentos = ObterNotasComponente(componenteCurricularCodigo, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = ObterNotaPosConselho(componenteCurricularCodigo, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno)
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