using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaPeriodoDto
    {
        public string Descricao { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
        public List<RespostaDto> Respostas { get; set; }
    }
}