using System;

namespace SME.SGP.Infra
{
    public class QuantidadeEncaminhamentoNAAPAEmAbertoDto
    {
        public string CodigoDre { get; set; }
        public string DescricaoDre { get; set; }
        public long Quantidade { get; set; }
        public DateTime DataUltimaConsolidacao { get; set; }
    }
}
