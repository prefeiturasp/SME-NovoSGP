using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaResumoFrequenciaDto
    {
        public IEnumerable<RecuperacaoParalelaTotalFrequenciaAnoDto> Anos { get; set; }
        public IEnumerable<RecuperacaoParalelaTotalFrequenciaCicloDto> Ciclos { get; set; }
        public double PorcentagemTotalFrequencia { get; set; }
        public int QuantidadeTotalFrequencia { get; set; }
    }
}