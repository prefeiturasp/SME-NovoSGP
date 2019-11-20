using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class EventosAulasTipoCalendarioDto
    {
        public int Dia { get; set; }
        public int QuantidadeDeEventosAulas { get; set; }
        public List<string> TiposEvento { get; set; }
    }
}