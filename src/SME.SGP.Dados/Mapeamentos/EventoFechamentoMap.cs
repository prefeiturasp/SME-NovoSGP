using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class EventoFechamentoMap: BaseMap<EventoFechamento>
    {
        public EventoFechamentoMap()
        {
            ToTable("evento_fechamento");
            Map(x => x.EventoId).ToColumn("evento_id");
            Map(x => x.FechamentoId).ToColumn("fechamento_id");
        }
    }
}
