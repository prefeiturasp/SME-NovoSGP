using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalAprovacaoDto
    {
        public string Modalidade { get; set; }
        public List<IndicadorAprovacaoDto> Indicadores { get; set; }
    }

    public class IndicadorAprovacaoDto
    {
        public string SerieAno { get; set; }
        public int TotalPromocoes { get; set; }
        public int TotalRetencoesAusencias { get; set; }
        public int TotalRetencoesNotas { get; set; }
    }

}
