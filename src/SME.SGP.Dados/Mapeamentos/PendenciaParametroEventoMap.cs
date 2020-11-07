using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PendenciaParametroEventoMap : BaseMap<PendenciaParametroEvento>
    {
        public PendenciaParametroEventoMap()
        {
            ToTable("pendencia_parametro_evento");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
            Map(c => c.ParametroSistemaId).ToColumn("parametro_sistema_id");
        }
    }
}
