using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class FrequenciaAluno : EntidadeBase
    {
        public const int PERCENTUAL_FREQUENCIA_PRECISAO = 2;

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
            int totalPresencas,
            string professor)
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
            Professor = professor;
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

                var porcentagem = 100 - (((double)NumeroFaltasNaoCompensadas / TotalAulas) * 100);

                return ArredondarPercentual(porcentagem > 100 ? 100 : porcentagem);
            }
        }

        public string PercentualFrequenciaFormatado => TotalAulas > 0 ? FormatarPercentual(PercentualFrequencia) : string.Empty;

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
        public string Professor { get; set; }

        /// <summary>
        /// Lista montada para particularidade de cálculo para o ano de 2020.
        /// </summary>
        public ICollection<(int bimestre, double percentual)> PercentuaisFrequenciaPorBimestre { get; private set; }

        /// <summary>
        /// Cálculo de percentual final específico para 2020.
        /// </summary>
        public double PercentualFrequenciaFinal
        {
            get
            {
                return ArredondarPercentual(PercentuaisFrequenciaPorBimestre.Any() ? PercentuaisFrequenciaPorBimestre.Sum(p => p.percentual) / PercentuaisFrequenciaPorBimestre.Count : 0);
            }
        }

        public string PercentualFrequenciaFinalFormatado => FormatarPercentual(PercentualFrequenciaFinal);

        public FrequenciaAluno DefinirFrequencia(string disciplinaId, int totalAusencias, int totalAulas, int totalCompensacoes, TipoFrequenciaAluno tipoFrequencia, int totalRemotos, int totalPresencas, string professor)
        {
            DisciplinaId = disciplinaId;
            Tipo = tipoFrequencia;
            TotalAusencias = totalAusencias;
            TotalCompensacoes = totalCompensacoes;
            TotalAulas = totalAulas;
            TotalRemotos = totalRemotos;
            TotalPresencas = totalPresencas;
            AlteradoEm = DateTime.Now;
            Professor = professor;

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

        public static double ArredondarPercentual(double percentual) => Math.Round(percentual, PERCENTUAL_FREQUENCIA_PRECISAO);

        public static string FormatarPercentual(double percentual)
        {
            return percentual.ToString($"N{PERCENTUAL_FREQUENCIA_PRECISAO}", CultureInfo.CurrentCulture);
        }
    }
}