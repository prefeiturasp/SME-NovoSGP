using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaTotalResultadoDto
    {
        public string EixoDescricao { get; set; }
        public IEnumerable<RecuperacaoParalelaResumoResultadoObjetivoDto> Objetivos { get; set; }
    }
}