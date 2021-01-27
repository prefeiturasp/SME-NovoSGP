using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PendenciaCalendarioUeMap : BaseMap<PendenciaCalendarioUe>
    {
        public PendenciaCalendarioUeMap()
        {
            ToTable("pendencia_calendario_ue");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
        }
    }
}
