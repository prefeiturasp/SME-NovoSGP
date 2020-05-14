using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class DiaEventoAula
    {
        public IEnumerable<EventosAulasTipoDiaDto> EventosAulas { get; set; }
        public bool Letivo { get; set; }
        public bool DentroPeriodo { get; set; }
    }
}