using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCalculoFrequencia : IServicoCalculoFrequencia
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;
        private readonly IServicoEOL servicoEOL;

        public ServicoCalculoFrequencia(IServicoEOL servicoEOL,
                                        IRepositorioFrequencia repositorioFrequencia,
                                        IRepositorioAula repositorioAula,
                                        IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno,
                                        IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                        IRepositorioFrequenciaAlunoDisciplinaPeriodo repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }

        public void CalcularFrequenciaPorTurmaEDisciplina(IEnumerable<string> alunos, long aulaId)
        {
            var aula = repositorioAula.ObterPorId(aulaId);
            if (aula == null)
            {
                throw new NegocioException("Aula não encontrada ao calcular percentual de frequência.");
            }
            if (alunos == null || !alunos.Any())
            {
                throw new NegocioException("A lista de alunos a turma e a disciplina devem ser informados para calcular a frequência.");
            }

            var dataAtual = DateTime.Now;
            var totalAulas = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplinaETurma(dataAtual, aula.DisciplinaId, aula.TurmaId);

            foreach (var codigoAluno in alunos)
            {
                var ausenciasAluno = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoEDisciplina(dataAtual, codigoAluno, aula.DisciplinaId, aula.TurmaId);
                if (ausenciasAluno != null)
                {
                    var frequenciaAluno = MapearFrequenciaAluno(codigoAluno,
                                                                aula.DisciplinaId,
                                                                ausenciasAluno.PeriodoInicio,
                                                                ausenciasAluno.PeriodoFim,
                                                                ausenciasAluno.Bimestre,
                                                                ausenciasAluno.TotalAusencias,
                                                                totalAulas);

                    if (frequenciaAluno.PercentualFrequencia < 100)
                        repositorioFrequenciaAlunoDisciplinaPeriodo.Salvar(frequenciaAluno);
                }
            }
        }

        public void CalcularPercentualFrequenciaAlunosPorDisciplinaEPeriodo(int anoLetivo)
        {
            //CalcularFrequenciaPorAluno(anoLetivo);
        }

        //private void CalcularFrequenciaPorAluno(int anoLetivo)
        //{
        //    var ausenciasPorTurmaEAno = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoEDisciplina(anoLetivo);
        //    if (ausenciasPorTurmaEAno != null && ausenciasPorTurmaEAno.Any())
        //    {
        //        var aulasPorDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplina(anoLetivo);
        //        if (aulasPorDisciplina != null && aulasPorDisciplina.Any())
        //        {
        //            foreach (var ausencia in ausenciasPorTurmaEAno)
        //            {
        //                int totalAulas = ObterTotalAulasPorDisciplina(aulasPorDisciplina, ausencia);
        //                var frequenciaAluno = MapearFrequenciaAluno(ausencia, totalAulas);
        //                if (frequenciaAluno.PercentualFrequencia < 100)
        //                    repositorioFrequenciaAlunoDisciplinaPeriodo.Salvar(frequenciaAluno);
        //            }
        //        }
        //        var ausenciasPorAluno = ausenciasPorTurmaEAno.GroupBy(c => c.CodigoAluno);
        //        foreach (var ausenciaAluno in ausenciasPorAluno)
        //        {
        //            foreach (var ausencia in ausenciaAluno)
        //            {
        //                int totalAulas = ObterTotalAulasPorDisciplina(aulasPorDisciplina, ausencia);
        //                var totalAulasGeral = aulasPorDisciplina.Where(c => c.DisciplinaId == ausencia.DisciplinaId).Sum(c => c.TotalAulas);
        //                var ausenciasAlunoTotal = ausenciaAluno.Sum(c => c.TotalAusencias);
        //                var frequenciaGeral = 100 - ((ausenciasAlunoTotal / totalAulasGeral) * 100);
        //            }
        //        }
        //    }
        //}

        private FrequenciaAlunoDisciplinaPeriodo MapearFrequenciaAluno(string codigoAluno, string disciplinaId, DateTime periodoInicio, DateTime periodoFim, int bimestre, int totalAusencias, int totalAulas)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.Obter(codigoAluno, disciplinaId, periodoInicio, periodoFim);
            return frequenciaAluno == null ?
            new FrequenciaAlunoDisciplinaPeriodo
                         (
                             codigoAluno,
                             disciplinaId,
                             periodoInicio,
                             periodoFim,
                             bimestre,
                             totalAusencias,
                             totalAulas
                         ) : frequenciaAluno.DefinirFrequencia(totalAusencias, totalAulas);
        }

        //private int ObterTotalAulasPorDisciplina(IEnumerable<AulasPorDisciplinaDto> aulasPorDisciplina, AusenciaPorDisciplinaDto ausencia)
        //{
        //    var totalAulas = aulasPorDisciplina.FirstOrDefault(c => c.DisciplinaId == ausencia.DisciplinaId)?.TotalAulas;
        //    if (totalAulas == null)
        //    {
        //        throw new NegocioException($"Ocorreu um erro ao localizar as aulas da disciplina com código: {ausencia.DisciplinaId}");
        //    }

        //    return totalAulas.Value;
        //}
    }
}