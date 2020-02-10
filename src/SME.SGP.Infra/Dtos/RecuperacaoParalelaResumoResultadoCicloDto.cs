using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class RecuperacaoParalelaResumoResultadoCicloDto
    {
        public string CicloDescricao { get; set; }
        public IEnumerable<RecuperacaoParalelaResumoResultadoRespostaDto> Respostas { get; set; }
    }
}