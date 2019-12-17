using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class EventosAulasTipoCalendarioDto
    {
        public int Dia { get; set; }
        public int QuantidadeDeEventosAulas { get; set; }
        public bool TemEvento { get; set; }
        public bool TemAula { get; set; }
        public bool TemAulaCJ { get; set; }
        public bool TemAtividadeAvaliativa { get; set; }
    }
}