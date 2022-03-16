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
            Map(c => c.EventoId).ToColumn("evento_id");
            Map(c => c.FechamentoId).ToColumn("fechamento_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
