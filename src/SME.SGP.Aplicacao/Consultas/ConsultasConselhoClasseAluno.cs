using SME.SGP.Aplicacao.Integracoes;
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
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasFechamentoTurma consultasFechamentoTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IConsultasFrequencia consultasFrequencia;

        public ConsultasConselhoClasseAluno(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                            IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                            IConsultasFechamentoTurma consultasFechamentoTurma,
                                            IServicoEOL servicoEOL,
                                            IServicoUsuario servicoUsuario,
                                            IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo,
                                            IConsultasFrequencia consultasFrequencia)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
        }

        public async Task<bool> ExisteConselhoClasseUltimoBimestreAsync(Turma turma, string alunoCodigo)
        {
            var periodoEscolar = await ObterPeriodoUltimoBimestre(turma);

            var conselhoClasseUltimoBimestre = await repositorioConselhoClasseAluno.ObterPorPeriodo(alunoCodigo, turma.Id, periodoEscolar.Id);
            return conselhoClasseUltimoBimestre != null;
        }

        private async Task<PeriodoEscolar> ObterPeriodoUltimoBimestre(Turma turma)
        {
            var periodoEscolarUltimoBimestre = await consultasPeriodoEscolar.ObterUltimoPeriodoAsync(turma.AnoLetivo, turma.ModalidadeTipoCalendario, turma.Semestre);
            if (periodoEscolarUltimoBimestre == null)
                throw new NegocioException("Não foi possível localizar o período escolar do ultimo bimestre da turma");

            return periodoEscolarUltimoBimestre;
        }

        public async Task<ConselhoClasseAluno> ObterPorConselhoClasseAsync(long conselhoClasseId, string alunoCodigo)
            => await repositorioConselhoClasseAluno.ObterPorConselhoClasseAsync(conselhoClasseId, alunoCodigo);

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

            var disciplinas = await servicoEOL.ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(fechamentoTurma.Turma.CodigoTurma, usuario.Login, usuario.PerfilAtual);

            if (disciplinas == null || !disciplinas.Any())
                return null;

            var disciplinasSinteses = disciplinas.Where(x => !x.BaseNacional && x.GrupoMatriz != null);

            if (disciplinasSinteses == null || !disciplinasSinteses.Any())
                return null;

            var frequenciaAluno = await repositorioFrequenciaAlunoDisciplinaPeriodo.ObterFrequenciaBimestres(alunoCodigo, bimestre, fechamentoTurma.Turma.CodigoTurma);

            foreach (var componenteCurricular in disciplinasSinteses)
            {
                var grupoSintese = retorno.FirstOrDefault(x => x.Id == componenteCurricular.Codigo);

                MapearDto(retorno, ref grupoSintese, frequenciaAluno, componenteCurricular);
            }

            return retorno;
        }

        private void MapearDto(IEnumerable<ConselhoDeClasseGrupoMatrizDto> retorno, ref ConselhoDeClasseGrupoMatrizDto grupoSintese, IEnumerable<FrequenciaAluno> frequenciaAluno, ComponenteCurricularEol componenteCurricular)
        {
            var frequenciaDisciplina = ObterFrequenciaPorDisciplina(frequenciaAluno, componenteCurricular);

            var percentualFrequencia = ObterPercentualDeFrequencia(frequenciaDisciplina);

            var dto = MapeaderDisciplinasDto(componenteCurricular);

            var parecerFinal = consultasFrequencia.ObterSinteseAluno(percentualFrequencia, dto);

            var componenteSinteseAdicionar = MapearConselhoDeClasseComponenteSinteseDto(componenteCurricular, frequenciaDisciplina, percentualFrequencia, parecerFinal);

            if (grupoSintese != null)
            {
                grupoSintese.ComponenteSinteses.ToList().Add(componenteSinteseAdicionar);
                return;
            }

            grupoSintese = Mapear(componenteCurricular, componenteSinteseAdicionar);

            retorno.ToList().Add(grupoSintese);
        }

        private static ConselhoDeClasseGrupoMatrizDto Mapear(ComponenteCurricularEol componenteCurricular, ConselhoDeClasseComponenteSinteseDto componenteSinteseAdicionar)
        {
            return new ConselhoDeClasseGrupoMatrizDto
            {
                Id = componenteCurricular.GrupoMatriz.Id,
                Titulo = componenteCurricular.GrupoMatriz.Nome,
                ComponenteSinteses = new List<ConselhoDeClasseComponenteSinteseDto>
                {
                   componenteSinteseAdicionar
                }
            };
        }

        private static ConselhoDeClasseComponenteSinteseDto MapearConselhoDeClasseComponenteSinteseDto(ComponenteCurricularEol componenteCurricular, IEnumerable<FrequenciaAluno> frequenciaDisciplina, double percentualFrequencia, SinteseDto parecerFinal)
        {
            return new ConselhoDeClasseComponenteSinteseDto
            {
                Codigo = componenteCurricular.Codigo,
                Nome = componenteCurricular.Descricao,
                TotalFaltas = frequenciaDisciplina.Sum(x => x.TotalAusencias),
                PercentualFrequencia = percentualFrequencia,
                ParecerFinal = parecerFinal.SinteseNome,
                ParecerFinalId = (int)parecerFinal.SinteseId
            };
        }

        private static DisciplinaDto MapeaderDisciplinasDto(ComponenteCurricularEol componenteCurricular)
        {
            return new DisciplinaDto
            {
                CodigoComponenteCurricular = componenteCurricular.Codigo,
                Compartilhada = componenteCurricular.Compartilhada,
                LancaNota = componenteCurricular.LancaNota,
                Nome = componenteCurricular.Descricao,
                PossuiObjetivos = componenteCurricular.PossuiObjetivos,
                Regencia = componenteCurricular.Regencia,
                RegistraFrequencia = componenteCurricular.RegistraFrequencia,
                TerritorioSaber = componenteCurricular.TerritorioSaber
            };
        }

        private static IEnumerable<FrequenciaAluno> ObterFrequenciaPorDisciplina(IEnumerable<FrequenciaAluno> frequenciaAluno, ComponenteCurricularEol componenteCurricular)
        {
            return frequenciaAluno.Where(x => x.DisciplinaId == componenteCurricular.Codigo.ToString());
        }

        private static double ObterPercentualDeFrequencia(IEnumerable<FrequenciaAluno> frequenciaDisciplina)
        {
            return frequenciaDisciplina.Any() ? frequenciaDisciplina.Sum(x => x.PercentualFrequencia) / frequenciaDisciplina.Count() : 100;
        }

        public async Task<ParecerConclusivoDto> ObterParecerConclusivo(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo)
        {
            var fechamentoTurma = await consultasFechamentoTurma.ObterCompletoPorIdAsync(fechamentoTurmaId);
            if (!await ExisteConselhoClasseUltimoBimestreAsync(fechamentoTurma.Turma, alunoCodigo))
                throw new NegocioException("Aluno não possui conselho de classe do último bimestre");

            // TODO 12233 consultar parecer conclusivo
            return new ParecerConclusivoDto()
            {
                ParecerConclusivoCodigo = ParecerConclusivo.Aprovado,
                ParecerConclusivoNome = ParecerConclusivo.Aprovado.Name()
            };
        }
    }
}