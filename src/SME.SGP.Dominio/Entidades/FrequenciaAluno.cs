using System;

namespace SME.SGP.Dominio
{
    public class FrequenciaAluno : EntidadeBase
    {
        public FrequenciaAluno(string codigoAluno,
                 string turmaId,
                 string disciplinaId,
                 DateTime periodoInicio,
                 DateTime periodoFim,
                 int bimestre,
                 int totalAusencias,
                 int totalAulas,
                 int totalCompensacoes,
                 TipoFrequenciaAluno tipo)
        {
            Bimestre = bimestre;
            CodigoAluno = codigoAluno;
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            PeriodoFim = periodoFim;
            PeriodoInicio = periodoInicio;
            TotalAulas = totalAulas;
            TotalCompensacoes = totalCompensacoes;
            Tipo = tipo;
            TotalAusencias = totalAusencias;
        }

        protected FrequenciaAluno()
        {
        }

        public int Bimestre { get; set; }
        public string CodigoAluno { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public double PercentualFrequencia => 100 - ((NumeroFaltasNaoCompensadas / TotalAulas) * 100);
        public DateTime PeriodoFim { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public TipoFrequenciaAluno Tipo { get; set; }
        public double TotalAulas { get; set; }
        public double TotalAusencias { get; set; }
        public double TotalCompensacoes { get; set; }

        public double NumeroFaltasNaoCompensadas { get => TotalAusencias - TotalCompensacoes; }

        public FrequenciaAluno DefinirFrequencia(int totalAusencias, int totalAulas, int totalCompensacoes, TipoFrequenciaAluno tipoFrequencia)
        {
            Tipo = tipoFrequencia;
            TotalAusencias = totalAusencias;
            TotalCompensacoes = totalCompensacoes;
            TotalAulas = totalAulas;
            return this;
        }
    }
}