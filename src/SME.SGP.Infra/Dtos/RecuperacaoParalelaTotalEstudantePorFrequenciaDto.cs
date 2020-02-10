using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaTotalEstudantePorFrequenciaDto
    {
        public IEnumerable<RecuperacaoParalelaResumoFrequenciaDto> Frequencia { get; set; }
        public double PorcentagemTotal { get; set; }
        public int QuantidadeTotal { get; set; }
    }
}