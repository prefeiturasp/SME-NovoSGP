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

        public void CalcularFrequenciaPorTurma(IEnumerable<string> alunos, long aulaId)
        {
            Aula aula = ObterAula(alunos, aulaId);

            var totalAulasNaDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(aula.DataAula, aula.DisciplinaId, aula.TurmaId);
            var totalAulasDaTurmaGeral = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(aula.DataAula, string.Empty, aula.TurmaId);

            foreach (var codigoAluno in alunos)
            {
                RegistraFrequenciaPorDisciplina(aula, aula.DataAula, totalAulasNaDisciplina, codigoAluno);
                RegistraFrequenciaGeral(aula, aula.DataAula, codigoAluno, totalAulasDaTurmaGeral);
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

        private Aula ObterAula(IEnumerable<string> alunos, long aulaId)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
            {
                throw new NegocioException("Aula não encontrada ao calcular percentual de frequência.");
            }
            if (alunos == null || !alunos.Any())
            {
                throw new NegocioException("A lista de alunos a turma e o componente curricular devem ser informados para calcular a frequência.");
            }

            return aula;
        }

        private void RegistraFrequenciaGeral(Aula aula, DateTime dataAtual, string codigoAluno, int totalAulasDaTurma)
        {
            var totalAusenciasGeralAluno = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurma(dataAtual, codigoAluno, string.Empty, aula.TurmaId);
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

        private void RegistraFrequenciaPorDisciplina(Aula aula, DateTime dataAtual, int totalAulasNaDisciplina, string codigoAluno)
        {
            var ausenciasAlunoPorDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoETurma(dataAtual, codigoAluno, aula.DisciplinaId, aula.TurmaId);
            if (ausenciasAlunoPorDisciplina != null)
            {
                var frequenciaAluno = MapearFrequenciaAluno(codigoAluno,
                                                            aula.DisciplinaId,
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