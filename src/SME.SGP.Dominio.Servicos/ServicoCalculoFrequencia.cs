using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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

        public void CalcularPercentualFrequenciaAlunosPorDisciplinaEPeriodo(int anoLetivo)
        {
            CalcularFrequenciaPorAluno(anoLetivo);
        }

        private void CalcularFrequenciaPorAluno(int anoLetivo)
        {
            var aulasPorDisciplina = repositorioRegistroAusenciaAluno.ObterTotalAulasPorDisciplina(anoLetivo);
            if (aulasPorDisciplina != null && aulasPorDisciplina.Any())
            {
                var ausenciasPorTurmaEAno = repositorioRegistroAusenciaAluno.ObterTotalAusenciasPorAlunoEDisciplina(anoLetivo);
                if (ausenciasPorTurmaEAno != null && ausenciasPorTurmaEAno.Any())
                {
                    foreach (var ausencia in ausenciasPorTurmaEAno)
                    {
                        int totalAulas = ObterTotalAulasPorDisciplina(aulasPorDisciplina, ausencia);
                        var frequenciaAluno = MapearFrequenciaAluno(ausencia, totalAulas);
                        if (frequenciaAluno.PercentualFrequencia < 100)
                            repositorioFrequenciaAlunoDisciplinaPeriodo.Salvar(frequenciaAluno);
                    }
                }
                //var ausenciasPorAluno = ausenciasPorTurmaEAno.GroupBy(c => c.CodigoAluno);
                //foreach (var ausenciaAluno in ausenciasPorAluno)
                //{
                //    var totalAulasGeral = aulasPorDisciplina.Sum(c => c.TotalAulas);
                //    var ausenciasAlunoTotal = ausenciaAluno.Sum(c => c.TotalAusencias);
                //    var frequenciaGeral = 100 - ((ausenciasAlunoTotal / totalAulasGeral) * 100);
                //}
            }
        }

        private FrequenciaAlunoDisciplinaPeriodo MapearFrequenciaAluno(AusenciaPorDisciplinaDto ausencia, int totalAulas)
        {
            var frequenciaAluno = repositorioFrequenciaAlunoDisciplinaPeriodo.Obter(ausencia.CodigoAluno, ausencia.DisciplinaId, ausencia.PeriodoInicio, ausencia.PeriodoFim);
            return frequenciaAluno == null ?
            new FrequenciaAlunoDisciplinaPeriodo
                         (
                             ausencia.CodigoAluno,
                             ausencia.DisciplinaId,
                             ausencia.PeriodoInicio,
                             ausencia.PeriodoFim,
                             ausencia.Bimestre,
                             ausencia.TotalAusencias,
                             totalAulas
                         ) : frequenciaAluno.DefinirFrequencia(ausencia.TotalAusencias, totalAulas);
        }

        private int ObterTotalAulasPorDisciplina(IEnumerable<AulasPorDisciplinaDto> aulasPorDisciplina, AusenciaPorDisciplinaDto ausencia)
        {
            var totalAulas = aulasPorDisciplina.FirstOrDefault(c => c.DisciplinaId == ausencia.DisciplinaId)?.TotalAulas;
            if (totalAulas == null)
            {
                throw new NegocioException($"Ocorreu um erro ao localizar as aulas da disciplina com código: {ausencia.DisciplinaId}");
            }

            return totalAulas.Value;
        }
    }
}