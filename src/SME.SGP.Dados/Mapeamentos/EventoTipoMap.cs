using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoTipoMap : BaseMap<EventoTipo>
    {
        public EventoTipoMap()
        {
            ToTable("evento_tipo");
            Map(e => e.LocalOcorrencia).ToColumn("local_ocorrencia");
            Map(e => e.TipoData).ToColumn("tipo_data");
            Map(e => e.SomenteLeitura).ToColumn("somente_leitura");
        }
    }
}
