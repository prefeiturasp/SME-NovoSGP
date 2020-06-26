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
        private readonly IConsultasDisciplina consultasDisciplina;
        private readonly IConsultasFechamentoNota consultasFechamentoNota;
        private readonly IConsultasFechamentoTurma consultasFechamentoTurma;
        private readonly IConsultasFrequencia consultasFrequencia;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAluno;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IServicoConselhoClasse servicoConselhoClasse;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasConselhoClasseAluno(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAluno,
                                            IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                            IConsultasTipoCalendario consultasTipoCalendario,
                                            IConsultasFechamentoTurma consultasFechamentoTurma,
                                            IConsultasAulaPrevista consultasAulaPrevista,
                                            IConsultasConselhoClasseNota consultasConselhoClasseNota,
                                            IConsultasFechamentoNota consultasFechamentoNota,
                                            IConsultasDisciplina consultasDisciplina,
                                            IServicoEol servicoEOL,
                                            IServicoUsuario servicoUsuario,
                                            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                            IRepositorioAula repositorioAula,
                                            IConsultasFrequencia consultasFrequencia,
                                            IServicoConselhoClasse servicoConselhoClasse)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioFrequenciaAluno = repositorioFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAluno));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new ArgumentNullException(nameof(consultasTipoCalendario));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
            this.consultasAulaPrevista = consultasAulaPrevista ?? throw new ArgumentNullException(nameof(consultasAulaPrevista));
            this.consultasConselhoClasseNota = consultasConselhoClasseNota ?? throw new ArgumentNullException(nameof(consultasConselhoClasseNota));
            this.consultasFechamentoNota = consultasFechamentoNota ?? throw new ArgumentNullException(nameof(consultasFechamentoNota));
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
            this.servicoConselhoClasse = servicoConselhoClasse ?? throw new ArgumentNullException(nameof(servicoConselhoClasse));
        }

        public async Task<bool> ExisteConselhoClasseUltimoBimestreAsync(Turma turma, string alunoCodigo)
        {
            var periodoEscolar = await ObterPeriodoUltimoBimestre(turma);

            var conselhoClasseUltimoBimestre = await repositorioConselhoClasseAluno.ObterPorPeriodoAsync(alunoCodigo, turma.Id, periodoEscolar.Id);
            if (conselhoClasseUltimoBimestre == null)
                return false;

            return await servicoConselhoClasse.VerificaNotasTodosComponentesCurriculares(alunoCodigo, turma, periodoEscolar.Id);
        }

        public async Task<IEnumerable<ConselhoDeClasseGrupoMatrizDto>> ObterListagemDeSinteses(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var retorno = new List<ConselhoDeClasseGrupoMatrizDto>();

            var fechamentoTurma = await consultasFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);

            if (fechamentoTurma == null)
                throw new NegocioException("Não existe fechamento para a turma");

            var bimestre = fechamentoTurma.PeriodoEscolar?.Bimestre ?? 0;

            if (bimestre == 0 && !await ExisteConselhoClasseUltimoBimestreAsync(fechamentoTurma.Turma, alunoCodigo))
                throw new NegocioException("Aluno não possui conselho de classe do último bimestre");

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurma(fechamentoTurma.Turma.CodigoTurma);
            if (disciplinas == null || !disciplinas.Any())
                return null;

            var gruposMatrizes = disciplinas.Where(x => !x.LancaNota && x.GrupoMatriz != null)
                                            .GroupBy(c => new { Id = c.GrupoMatriz?.Id, Nome = c.GrupoMatriz?.Nome });

            var frequenciaAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaBimestresAsync(alunoCodigo, bimestre, fechamentoTurma.Turma.CodigoTurma);

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
                    var componenteCurricularDto = MapearDto(frequenciaAluno, componenteCurricular, bimestre);
                    grupoMatriz.ComponenteSinteses.Add(componenteCurricularDto);
                }

                retorno.Add(grupoMatriz);
            }

            return retorno;
        }

        public async Task<ConselhoClasseAlunoNotasConceitosRetornoDto> ObterNotasFrequencia(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var fechamentoTurma = await consultasFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);
            if (fechamentoTurma == null)
                throw new NegocioException("Fechamento da Turma não localizado");

            var turmaCodigo = fechamentoTurma.Turma.CodigoTurma;

            var notasConselhoClasseAluno = await consultasConselhoClasseNota.ObterNotasAlunoAsync(conselhoClasseId, alunoCodigo);
            var notasFechamentoAluno = fechamentoTurma.PeriodoEscolarId.HasValue ?
                await consultasFechamentoNota.ObterNotasAlunoBimestreAsync(fechamentoTurmaId, alunoCodigo) :
                await consultasConselhoClasseNota.ObterNotasFinaisBimestresAlunoAsync(alunoCodigo, turmaCodigo);

            var disciplinas = await servicoEOL.ObterDisciplinasPorCodigoTurma(turmaCodigo);
            if (disciplinas == null)
                throw new NegocioException("Disciplinas da turma não localizadas no EOL");

            var retorno = new ConselhoClasseAlunoNotasConceitosRetornoDto();

            var gruposMatrizesNotas = new List<ConselhoClasseAlunoNotasConceitosDto>();
            // Retornar componentes que lançam nota
            var gruposMatrizes = disciplinas.Where(c => c.LancaNota).GroupBy(c => c.GrupoMatriz?.Nome);
            foreach (var grupoDisiplinasMatriz in gruposMatrizes.OrderBy(k => k.Key))
            {
                var conselhoClasseAlunoNotas = new ConselhoClasseAlunoNotasConceitosDto();
                conselhoClasseAlunoNotas.GrupoMatriz = grupoDisiplinasMatriz.Key;

                foreach (var disciplina in grupoDisiplinasMatriz)
                {
                    // Carrega Frequencia Aluno
                    var frequenciaAluno = await ObterFrequenciaAluno(fechamentoTurma.Turma,
                                                                     fechamentoTurma.PeriodoEscolar,
                                                                     disciplina.CodigoComponenteCurricular,
                                                                     alunoCodigo);

                    if (disciplina.Regencia)
                        conselhoClasseAlunoNotas.ComponenteRegencia = await ObterNotasFrequenciaRegencia(disciplina,
                                                                                                         frequenciaAluno,
                                                                                                         fechamentoTurma.PeriodoEscolar,
                                                                                                         fechamentoTurma.Turma,
                                                                                                         notasConselhoClasseAluno,
                                                                                                         notasFechamentoAluno);
                    else
                        conselhoClasseAlunoNotas.ComponentesCurriculares.Add(ObterNotasFrequenciaComponente(disciplina,
                                                                                                                  frequenciaAluno,
                                                                                                                  fechamentoTurma.PeriodoEscolar,
                                                                                                                  fechamentoTurma.Turma,
                                                                                                                  notasConselhoClasseAluno,
                                                                                                                  notasFechamentoAluno));
                }
                gruposMatrizesNotas.Add(conselhoClasseAlunoNotas);
            }

            retorno.PodeEditarNota = await VerificaSePodeEditarNota(alunoCodigo, fechamentoTurma);
            retorno.NotasConceitos = gruposMatrizesNotas;

            return retorno;
        }

        private async Task<bool> VerificaSePodeEditarNota(string alunoCodigo, FechamentoTurma fechamentoTurma)
        {
            var turmaFechamento = await servicoEOL.ObterAlunosPorTurma(fechamentoTurma.Turma.CodigoTurma, fechamentoTurma.Turma.AnoLetivo);
            
            if (turmaFechamento == null || !turmaFechamento.Any())
                throw new NegocioException($"Não foi possível obter os dados da turma {fechamentoTurma.Turma.CodigoTurma}");

            var aluno = turmaFechamento.FirstOrDefault(a => a.CodigoAluno == alunoCodigo);
            if (aluno == null)
                throw new NegocioException($"Não foi possível obter os dados do aluno {alunoCodigo}");
            return aluno.PodeEditarNotaConceitoNoPeriodo(fechamentoTurma.PeriodoEscolar);            
        }

        public async Task<ParecerConclusivoDto> ObterParecerConclusivo(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var fechamentoTurma = await consultasFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);
            if (!await ExisteConselhoClasseUltimoBimestreAsync(fechamentoTurma.Turma, alunoCodigo))
                throw new NegocioException("Aluno não possui conselho de classe do último bimestre");

            var conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(conselhoClasseId, alunoCodigo);
            if (conselhoClasseAluno == null || !conselhoClasseAluno.ConselhoClasseParecerId.HasValue)
                return await servicoConselhoClasse.GerarParecerConclusivoAlunoAsync(conselhoClasseId, fechamentoTurmaId, alunoCodigo);

            return new ParecerConclusivoDto()
            {
                Id = conselhoClasseAluno.ConselhoClasseParecerId.Value,
                Nome = conselhoClasseAluno.ConselhoClasseParecer.Nome
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
                ParecerFinal = parecerFinal?.SinteseNome ?? string.Empty,
                ParecerFinalId = (int)(parecerFinal?.SinteseId ?? default)
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

        private ConselhoDeClasseComponenteSinteseDto MapearDto(IEnumerable<FrequenciaAluno> frequenciaAluno, DisciplinaResposta componenteCurricular, int bimestre)
        {
            var frequenciaDisciplina = ObterFrequenciaPorDisciplina(frequenciaAluno, componenteCurricular);

            var percentualFrequencia = ObterPercentualDeFrequencia(frequenciaDisciplina);

            var dto = MapeaderDisciplinasDto(componenteCurricular);

            var parecerFinal = bimestre == 0 ? consultasFrequencia.ObterSinteseAluno(percentualFrequencia, dto) : null;

            var componenteSinteseAdicionar = MapearConselhoDeClasseComponenteSinteseDto(componenteCurricular, frequenciaDisciplina, percentualFrequencia, parecerFinal);
            return componenteSinteseAdicionar;
        }

        private async Task<FrequenciaAluno> ObterFrequenciaAluno(Turma turma, PeriodoEscolar periodoEscolar, long componenteCurricularCodigo, string alunoCodigo)
        {
            var frequenciaAluno = new FrequenciaAluno();
            if (periodoEscolar != null)
            {
                // Frequencia do bimestre
                frequenciaAluno = repositorioFrequenciaAluno.ObterPorAlunoData(alunoCodigo,
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
                    var frequenciaAlunoPeriodo = await repositorioFrequenciaAluno.ObterPorAlunoBimestreAsync(alunoCodigo,
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

        private NotaPosConselhoDto ObterNotaPosConselho(DisciplinaResposta componenteCurricular, int? bimestre, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var componenteCurricularCodigo = componenteCurricular.CodigoComponenteCurricular;
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

        private List<NotaBimestreDto> ObterNotasComponente(DisciplinaResposta componenteCurricular, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var notasFinais = new List<NotaBimestreDto>();

            if (periodoEscolar != null)
                notasFinais.Add(ObterNotaFinalComponentePeriodo(componenteCurricular.CodigoComponenteCurricular, periodoEscolar.Bimestre, notasFechamentoAluno));
            else
                notasFinais.AddRange(ObterNotasFinaisComponentePeriodos(componenteCurricular.CodigoComponenteCurricular, notasFechamentoAluno));

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

        private ConselhoClasseComponenteFrequenciaDto ObterNotasFrequenciaComponente(DisciplinaResposta disciplina, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var conselhoClasseComponente = new ConselhoClasseComponenteFrequenciaDto()
            {
                Nome = disciplina.Nome,
                CodigoComponenteCurricular = disciplina.CodigoComponenteCurricular,
                QuantidadeAulas = frequenciaAluno.TotalAulas,
                Faltas = frequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 100 : 100),
                NotasFechamentos = ObterNotasComponente(disciplina, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = ObterNotaPosConselho(disciplina, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno)
            };

            return conselhoClasseComponente;
        }

        private async Task<ConselhoClasseComponenteRegenciaFrequenciaDto> ObterNotasFrequenciaRegencia(DisciplinaResposta componenteCurricular, FrequenciaAluno frequenciaAluno, PeriodoEscolar periodoEscolar, Turma turma, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            var conselhoClasseComponente = new ConselhoClasseComponenteRegenciaFrequenciaDto()
            {
                QuantidadeAulas = frequenciaAluno.TotalAulas,
                Faltas = frequenciaAluno?.TotalAusencias ?? 0,
                AusenciasCompensadas = frequenciaAluno?.TotalCompensacoes ?? 0,
                Frequencia = (frequenciaAluno.TotalAulas > 0 ? frequenciaAluno?.PercentualFrequencia ?? 100 : 100)
            };

            var componentesRegencia = await consultasDisciplina.ObterComponentesRegencia(turma, componenteCurricular.CodigoComponenteCurricular);
            foreach (var componenteRegencia in componentesRegencia)
            {
                conselhoClasseComponente.ComponentesCurriculares.Add(ObterNotasRegencia(componenteRegencia, periodoEscolar, notasConselhoClasseAluno, notasFechamentoAluno));
            }

            return conselhoClasseComponente;
        }

        private ConselhoClasseNotasComponenteRegenciaDto ObterNotasRegencia(DisciplinaResposta componenteCurricular, PeriodoEscolar periodoEscolar, IEnumerable<NotaConceitoBimestreComponenteDto> notasConselhoClasseAluno, IEnumerable<NotaConceitoBimestreComponenteDto> notasFechamentoAluno)
        {
            return new ConselhoClasseNotasComponenteRegenciaDto()
            {
                Nome = componenteCurricular.Nome,
                CodigoComponenteCurricular = componenteCurricular.CodigoComponenteCurricular,
                NotasFechamentos = ObterNotasComponente(componenteCurricular, periodoEscolar, notasFechamentoAluno),
                NotaPosConselho = ObterNotaPosConselho(componenteCurricular, periodoEscolar?.Bimestre, notasConselhoClasseAluno, notasFechamentoAluno)
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