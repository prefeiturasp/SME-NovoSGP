using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PerfilEventoTipoMap : BaseMap<PerfilEventoTipo>
    {
        public PerfilEventoTipoMap()
        {
            ToTable("perfil_tipo_evento");
            Map(a => a.EventoTipoId).ToColumn("evento_tipo_id");
            Map(a => a.CodigoPerfil).ToColumn("codigo_perfil");
        }
    }
}
