using System;
using System.Globalization;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoBimestreDto
    {
        public const int PERCENTUAL_FREQUENCIA_PRECISAO = 2;
        public string Bimestre { get; set; }
        public int AulasPrevistas { get; set; }
        public int AulasRealizadas { get; set; }
        public int Ausencias { get; set; }
        public int Compensacoes { get; set; }
        public double? Frequencia { get; set; }
        public bool PossuiJustificativas { get; set; }
        public int Semestre { get; set; }
        public string FrequenciaFormatado => FormatarPercentual(Frequencia??0);

        public double CalcularPercentualFrequencia(int? TotalAulas = null, int? TotalFaltasNaoCompensadas = null)
        {
            if (!TotalAulas.HasValue)
                TotalAulas = AulasRealizadas;

            if (!TotalFaltasNaoCompensadas.HasValue)
                TotalFaltasNaoCompensadas = Ausencias - Compensacoes;

            if (TotalAulas == 0)
                return 0;

            var porcentagem = 100 - (((double)TotalFaltasNaoCompensadas / (double)TotalAulas) * 100);

            return ArredondarPercentual(porcentagem > 100 ? 100 : porcentagem);
            
        }
        public static double ArredondarPercentual(double percentual) => Math.Round(percentual, PERCENTUAL_FREQUENCIA_PRECISAO);

        public static string FormatarPercentual(double percentual)
        {
            return percentual.ToString($"N{PERCENTUAL_FREQUENCIA_PRECISAO}", CultureInfo.CurrentCulture);
        }
    }
}