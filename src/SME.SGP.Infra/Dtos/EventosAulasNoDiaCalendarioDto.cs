using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EventosAulasNoDiaCalendarioDto
    {
        public EventosAulasNoDiaCalendarioDto()
        {
            EventosAulas = new List<EventoAulaDto>();
        }
        public bool PodeCadastrarAula { get; set; }
        public IList<EventoAulaDto> EventosAulas { get; set; }        
        public string MensagemPeriodoEncerrado { get; set; }
    }
}
