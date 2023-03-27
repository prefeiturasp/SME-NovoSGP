using SME.SGP.Dominio;
using System.Globalization;

namespace SME.SGP.Infra
{
    public class DetalhamentoComponentesCurricularesAlunoDto
    {
        public const int PERCENTUAL_FREQUENCIA_PRECISAO = 2;
        public string NomeComponenteCurricular { get; set; }
        public string NotaFechamento { get; set; }
        public string NotaPosConselho { get; set; }
        public int QuantidadeAusencia { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public double PercentualFrequencia { get; set; }
        public bool LancaNota { get; set; }

        public string PercentualFrequenciaFormatado => FormatarPercentual(PercentualFrequencia);

        public static string FormatarPercentual(double percentual)
        {
            return percentual.ToString($"N{PERCENTUAL_FREQUENCIA_PRECISAO}", CultureInfo.CurrentCulture);
        }

    }
}
