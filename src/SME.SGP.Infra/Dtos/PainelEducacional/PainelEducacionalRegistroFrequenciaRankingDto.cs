using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalRegistroFrequenciaRankingDto
    {
        public IEnumerable<PainelEducacionalRegistroFrequenciaRankingItemDto> EscolasEmSituacaoCritica { get; set; }
        public IEnumerable<PainelEducacionalRegistroFrequenciaRankingItemDto> EscolasEmAtencao { get; set; }
        public IEnumerable<PainelEducacionalRegistroFrequenciaRankingItemDto> EscolasRanqueadas { get; set; }
    }

    public class PainelEducacionalRegistroFrequenciaRankingItemDto
    {
        public string Ue { get; set; }
        public decimal PercentualFrequencia { get; set; }
    }
}
