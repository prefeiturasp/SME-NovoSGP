using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaTotalResultadoDto
    {
        public IEnumerable<RecuperacaoParalelaResumoResultadoEixoDto> Eixos { get; set; }
    }
}