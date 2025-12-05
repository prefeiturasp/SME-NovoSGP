using System;

namespace SME.SGP.Infra
{
    public class AtendimentoNAAPAItineranciaAtendimentoDto
    {
        public long EncaminhamentoId { get; set; }
        public long SecaoEncaminhamentoNAAPAId { get; set; }
        public DateTime DataAtendimento { get; set; }
    }
}
