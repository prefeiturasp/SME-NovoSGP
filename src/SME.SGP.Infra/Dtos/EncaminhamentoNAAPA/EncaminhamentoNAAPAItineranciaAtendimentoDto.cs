using System;

namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPAItineranciaAtendimentoDto
    {
        public long EncaminhamentoId { get; set; }
        public long SecaoEncaminhamentoNAAPAId { get; set; }
        public DateTime DataAtendimento { get; set; }
    }
}
