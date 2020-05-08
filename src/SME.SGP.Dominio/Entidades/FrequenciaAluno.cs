using System;

namespace SME.SGP.Dominio
{
    public class FrequenciaAluno : EntidadeBase
    {
        public FrequenciaAluno(string codigoAluno,
                 string turmaId,
                 string disciplinaId,
                 long? periodoEscolarId,
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
            PeriodoEscolarId = periodoEscolarId;
            PeriodoFim = periodoFim;
            PeriodoInicio = periodoInicio;
            TotalAulas = totalAulas;
            TotalCompensacoes = totalCompensacoes;
            Tipo = tipo;
            TotalAusencias = totalAusencias;
        }

        public FrequenciaAluno()
        {
        }

        public int Bimestre { get; set; }
        public string CodigoAluno { get; set; }
        public string DisciplinaId { get; set; }
        public int NumeroFaltasNaoCompensadas { get => TotalAusencias - TotalCompensacoes; }
        public double PercentualFrequencia {
            get
            {
                if (TotalAulas == 0)
                    return 0;

                var porcentagem = 100 - ((double)NumeroFaltasNaoCompensadas / TotalAulas) * 100;
                
                return Math.Round(porcentagem, 2);
            }
        }
        public long? PeriodoEscolarId { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public TipoFrequenciaAluno Tipo { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalCompensacoes { get; set; }
        public string TurmaId { get; set; }

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