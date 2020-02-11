using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaTotalEstudantePorFrequenciaDto
    {
        public IEnumerable<RecuperacaoParalelaTotalEstudanteFrequenciaDto> Frequencia { get; set; }
    }
}