using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EventosAulasNoDiaCalendarioDto
    {
        public EventosAulasNoDiaCalendarioDto()
        {
            
        }
        public bool PodeCadastrarAula { get; set; }
        public IEnumerable<EventoAulaDto> EventosAulas { get; set; }        
        public string MensagemPeriodoEncerrado { get; set; }
        public IEnumerable<EventoAulaDiaDto> EventosAulasMes { get; set; }
    }
}
