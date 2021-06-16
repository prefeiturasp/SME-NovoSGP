using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoBimestreMap : BaseMap<EventoBimestre>
    {
        public EventoBimestreMap()
        {
            ToTable("evento_bimestre");
            Map(e => e.EventoId).ToColumn("evento_id");
            Map(e => e.Bimestre).ToColumn("bimestre");
        }
    }
}
