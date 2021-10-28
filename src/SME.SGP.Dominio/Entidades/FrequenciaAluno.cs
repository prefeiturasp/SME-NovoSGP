using System;
using System.Collections.Generic;
using System.Linq;

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
            TipoFrequenciaAluno tipo,
            int totalRemotos,
            int totalPresencas)
        {
            PercentuaisFrequenciaPorBimestre = new HashSet<(int, double)>();
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
            TotalRemotos = totalRemotos;
            TotalPresencas = totalPresencas;
        }

        public FrequenciaAluno()
        {
            PercentuaisFrequenciaPorBimestre = new HashSet<(int, double)>();
        }

        public int Bimestre { get; set; }
        public string CodigoAluno { get; set; }
        public string DisciplinaId { get; set; }

        public int NumeroFaltasNaoCompensadas
        {
            get => TotalAusencias - TotalCompensacoes;
        }

        public double PercentualFrequencia
        {
            get
            {
                if (TotalAulas == 0)
                    return 0;

                var porcentagem = 100 - ((double) NumeroFaltasNaoCompensadas / TotalAulas) * 100;

                return Math.Round(porcentagem > 100 ? 100 : porcentagem, 2);
            }
        }

        public long? PeriodoEscolarId { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public TipoFrequenciaAluno Tipo { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalCompensacoes { get; set; }
        public int TotalPresencas { get; set; }
        public int TotalRemotos { get; set; }
        public string TurmaId { get; set; }

        /// <summary>
        /// Lista montada para particularidade de cálculo para o ano de 2020.
        /// </summary>
        public ICollection<(int, double)> PercentuaisFrequenciaPorBimestre { get; private set; }

        /// <summary>
        /// Cálculo de percentual final específico para 2020.
        /// </summary>
        public double PercentualFrequenciaFinal
        {
            get
            {
                return PercentuaisFrequenciaPorBimestre.Any()
                    ? Math.Round(
                        PercentuaisFrequenciaPorBimestre.Sum(p => p.Item2) / PercentuaisFrequenciaPorBimestre.Count, 2)
                    : 100;
            }
        }

        public FrequenciaAluno DefinirFrequencia(int totalAusencias, int totalAulas, int totalCompensacoes,
            TipoFrequenciaAluno tipoFrequencia)
        {
            Tipo = tipoFrequencia;
            TotalAusencias = totalAusencias;
            TotalCompensacoes = totalCompensacoes;
            TotalAulas = totalAulas;
            return this;
        }

        /// <summary>
        /// Método de utilização exclusiva para o cálculo de frequência final de 2020.
        /// </summary>
        /// <param name="bimestre">Número do bimestre a ser adicionado na lista.</param>
        /// <param name="percentual">Percentual correspondente ao bimestre.</param>
        public void AdicionarFrequenciaBimestre(int bimestre, double percentual)
        {
            PercentuaisFrequenciaPorBimestre.Add((bimestre, percentual));
        }

        public bool FrequenciaNegativa()
            => (TotalAusencias - TotalCompensacoes) > TotalAulas;
    }
}