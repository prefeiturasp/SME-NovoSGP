using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunoAusenteDto
    {
        public const int PERCENTUAL_FREQUENCIA_PRECISAO = 2;
        public string Id { get; set; }
        public string Nome { get; set; }
        public int QuantidadeFaltasTotais { get; set; }
        public double? PercentualFrequencia { get; set; }
        public int MaximoCompensacoesPermitidas { get; set; }
        public bool Alerta { get; set; }
        public bool EhMatriculadoTurmaPAP { get; set; }
        public IEnumerable<CompensacaoDataAlunoDto> Compensacoes { get; set; }
        public string FrequenciaFormatado => FormatarPercentual(PercentualFrequencia ?? 0);

        public static string FormatarPercentual(double percentual)
        {
            return percentual.ToString($"N{PERCENTUAL_FREQUENCIA_PRECISAO}", CultureInfo.CurrentCulture);
        }
    }
}
