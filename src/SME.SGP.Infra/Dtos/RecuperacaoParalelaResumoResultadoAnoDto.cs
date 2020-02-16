using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaResumoResultadoAnoDto
    {
        public int AnoDescricao { get; set; }
        public IEnumerable<RecuperacaoParalelaResumoResultadoRespostaDto> Respostas { get; set; }
    }
}