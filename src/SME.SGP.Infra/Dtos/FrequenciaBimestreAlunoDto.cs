using System.Globalization;

namespace SME.SGP.Infra
{
    public class FrequenciaBimestreAlunoDto
    {
        public const int PERCENTUAL_FREQUENCIA_PRECISAO = 2;
        public string CodigoAluno { get; set; }
        public int Bimestre { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public int TotalAulas { get; set; }
        public double Frequencia { get; set; }
        public string FrequenciaFormatado => FormatarPercentual(Frequencia);

        public static string FormatarPercentual(double percentual)
        {
            return percentual.ToString($"N{PERCENTUAL_FREQUENCIA_PRECISAO}", CultureInfo.CurrentCulture);
        }
    }
}
