using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasConselhoClasseAluno : IConsultasConselhoClasseAluno
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;
        private readonly IConsultasFechamentoTurma consultasFechamentoTurma;

        public ConsultasConselhoClasseAluno(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                                            IConsultasPeriodoEscolar consultasPeriodoEscolar,
                                            IConsultasFechamentoTurma consultasFechamentoTurma)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
            this.consultasFechamentoTurma = consultasFechamentoTurma ?? throw new ArgumentNullException(nameof(consultasFechamentoTurma));
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