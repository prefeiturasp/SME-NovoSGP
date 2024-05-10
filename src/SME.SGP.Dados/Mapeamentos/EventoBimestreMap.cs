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
            Map(c => c.EventoId).ToColumn("evento_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
        }
    }
}

