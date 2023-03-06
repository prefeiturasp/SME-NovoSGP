using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPASecaoItineranciaQuestoesDto
    {
        public AuditoriaDto Auditoria { get; set; }
        public IEnumerable<QuestaoDto> Questoes { get; set; }
    }
}
