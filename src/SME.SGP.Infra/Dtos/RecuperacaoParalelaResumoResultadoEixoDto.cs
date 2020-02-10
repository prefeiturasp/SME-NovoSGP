using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaResumoResultadoEixoDto
    {
        public string EixoDescricao { get; set; }
        public IEnumerable<RecuperacaoParalelaResumoResultadoObjetivoDto> Objetivos { get; set; }
    }
}