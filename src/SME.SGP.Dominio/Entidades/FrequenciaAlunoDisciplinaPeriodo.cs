using System;

namespace SME.SGP.Dominio
{
    public class FrequenciaAlunoDisciplinaPeriodo : EntidadeBase
    {
        public FrequenciaAlunoDisciplinaPeriodo(string codigoAluno,
                                                string disciplinaId,
                                                DateTime periodoInicio,
                                                DateTime periodoFim,
                                                int bimestre,
                                                int totalAusencias,
                                                int totalAulas)
        {
            Bimestre = bimestre;
            CodigoAluno = codigoAluno;
            DisciplinaId = disciplinaId;
            PeriodoFim = periodoFim;
            PeriodoInicio = periodoInicio;
            TotalAulas = totalAulas;
            TotalAusencias = totalAusencias;
        }

        protected FrequenciaAlunoDisciplinaPeriodo()
        {
        }

        public int Bimestre { get; set; }
        public string CodigoAluno { get; set; }
        public string DisciplinaId { get; set; }
        public int PercentualFrequencia => 100 - ((TotalAusencias / TotalAulas) * 100);
        public DateTime PeriodoFim { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }

        public FrequenciaAlunoDisciplinaPeriodo DefinirFrequencia(int totalAusencias, int totalAulas)
        {
            TotalAusencias = totalAusencias;
            TotalAulas = totalAulas;
            return this;
        }
    }
}