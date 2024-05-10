using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaResumoTotalLihas
    {
        public IEnumerable<RecuperacaoParalelaTotalAnoDto> Anos { get; set; }
        public IEnumerable<RecuperacaoParalelaTotalCicloDto> Ciclos { get; set; }
    }
}