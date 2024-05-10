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
            Map(c => c.PendenciaCalendarioUeId).ToColumn("pendencia_calendario_ue_id");
            Map(c => c.ParametroSistemaId).ToColumn("parametro_sistema_id");
            Map(c => c.QuantidadeEventos).ToColumn("quantidade_eventos");
        }
    }
}
