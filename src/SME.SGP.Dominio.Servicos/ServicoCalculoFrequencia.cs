using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCalculoFrequencia : IServicoCalculoFrequencia
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;

        public ServicoCalculoFrequencia(IRepositorioAula repositorioAula,
                                        IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno,
                                        IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public void CalcularFrequenciaPorTurma(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId)
        {
            var totalAulasNaDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(dataAula, disciplinaId, turmaId);
            var totalAulasDaTurmaGeral = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(dataAula, string.Empty, turmaId);

            foreach (var codigoAluno in alunos)
            {
                RegistraFrequenciaPorDisciplina(turmaId, disciplinaId, dataAula, totalAulasNaDisciplina, codigoAluno);
                RegistraFrequenciaGeral(turmaId, dataAula, codigoAluno, totalAulasDaTurmaGeral);
            }
        }

        private FrequenciaAluno MapearFrequenciaAluno(string codigoAluno, string disciplinaId, DateTime periodoInicio, DateTime periodoFim, int bimestre, int totalAusencias, int totalAulas, TipoFrequenciaAluno tipo)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.Obter(codigoAluno, disciplinaId, periodoInicio, periodoFim, tipo);
            return frequenciaAluno == null ?
            new FrequenciaAluno
                         (
                             codigoAluno,
                             disciplinaId,
                             periodoInicio,
                             periodoFim,
                             bimestre,
                             totalAusencias,
                             totalAulas,
                             tipo
                         ) : frequenciaAluno.DefinirFrequencia(totalAusencias, totalAulas, tipo);
        }

        private void RegistraFrequenciaGeral(string turmaId, DateTime dataAtual, string codigoAluno, int totalAulasDaTurma)
        {
            var totalAusenciasGeralAluno = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurma(dataAtual, codigoAluno, string.Empty, turmaId);
            if (totalAusenciasGeralAluno != null)
            {
                var frequenciaGeralAluno = MapearFrequenciaAluno(codigoAluno,
                                                                    string.Empty,
                                                                    totalAusenciasGeralAluno.PeriodoInicio,
                                                                    totalAusenciasGeralAluno.PeriodoFim,
                                                                    totalAusenciasGeralAluno.Bimestre,
                                                                    totalAusenciasGeralAluno.TotalAusencias,
                                                                    totalAulasDaTurma,
                                                                    TipoFrequenciaAluno.Geral);
                if (frequenciaGeralAluno.PercentualFrequencia < 100)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Salvar(frequenciaGeralAluno);
            }
            else
            {
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(codigoAluno, dataAtual, TipoFrequenciaAluno.Geral);

                if (frequenciaAluno != null)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Remover(frequenciaAluno);
            }
        }

        private void RegistraFrequenciaPorDisciplina(string turmaId, string disciplinaId, DateTime dataAtual, int totalAulasNaDisciplina, string codigoAluno)
        {
            var ausenciasAlunoPorDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurma(dataAtual, codigoAluno, disciplinaId, turmaId);
            if (ausenciasAlunoPorDisciplina != null)
            {
                var frequenciaAluno = MapearFrequenciaAluno(codigoAluno,
                                                            disciplinaId,
                                                            ausenciasAlunoPorDisciplina.PeriodoInicio,
                                                            ausenciasAlunoPorDisciplina.PeriodoFim,
                                                            ausenciasAlunoPorDisciplina.Bimestre,
                                                            ausenciasAlunoPorDisciplina.TotalAusencias,
                                                            totalAulasNaDisciplina,
                                                            TipoFrequenciaAluno.PorDisciplina);

                if (frequenciaAluno.PercentualFrequencia < 100)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Salvar(frequenciaAluno);
            }
            else
            {
                var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.ObterPorAlunoData(codigoAluno, dataAtual, TipoFrequenciaAluno.PorDisciplina);

                if (frequenciaAluno != null)
                    repositorioFrequenciaAlunoDisciplinaPeriodo.Remover(frequenciaAluno);
            }
        }
    }
}