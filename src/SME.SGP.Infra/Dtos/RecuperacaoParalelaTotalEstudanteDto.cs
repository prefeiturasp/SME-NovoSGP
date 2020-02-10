using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaTotalEstudanteDto
    {
        public IEnumerable<RecuperacaoParalelaTotalAnoDto> Anos { get; set; }
        public IEnumerable<RecuperacaoParalelaTotalCicloDto> Ciclos { get; set; }
        public double PorcentagemTotal { get; set; }
        public int QuantidadeTotal { get; set; }
    }
}