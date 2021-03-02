using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PendenciaPlanoAEEMap : BaseMap<PendenciaPlanoAEE>
    {
        public PendenciaPlanoAEEMap()
        {
            ToTable("pendencia_plano_aee");
            Map(a => a.PendenciaId).ToColumn("pendencia_id");
            Map(a => a.PlanoAEEId).ToColumn("plano_aee_id");
        }
    }
}
