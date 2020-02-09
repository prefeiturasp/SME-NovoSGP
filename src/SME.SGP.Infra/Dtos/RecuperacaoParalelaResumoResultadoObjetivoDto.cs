using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaResumoResultadoObjetivoDto
    {
        public IEnumerable<RecuperacaoParalelaResumoResultadoAnoDto> Anos { get; set; }
        public IEnumerable<RecuperacaoParalelaResumoResultadoAnoDto> Ciclos { get; set; }
        public string ObjetivoDescricao { get; set; }
    }
}