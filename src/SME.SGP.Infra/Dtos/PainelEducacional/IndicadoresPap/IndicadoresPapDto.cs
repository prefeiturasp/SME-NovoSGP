using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap
{
    public class IndicadoresPapDto
    {
        public string NomeDificuldadeTop1 { get; set; }
        public string NomeDificuldadeTop2 { get; set; }
        public List<IndicadoresPapQuantidadesPorTipoDto> QuantidadesPorTipoPap { get; set; }
    }
}