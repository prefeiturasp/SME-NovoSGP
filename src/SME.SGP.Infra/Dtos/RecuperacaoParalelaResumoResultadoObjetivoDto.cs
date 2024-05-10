using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaResumoResultadoObjetivoDto
    {
        public IEnumerable<RecuperacaoParalelaResumoResultadoAnoDto> Anos { get; set; }
        public IEnumerable<RecuperacaoParalelaResumoResultadoCicloDto> Ciclos { get; set; }
        public string ObjetivoDescricao { get; set; }
        public IEnumerable<RecuperacaoParalelaResumoResultadoRespostaDto> Total { get; set; }
    }
}